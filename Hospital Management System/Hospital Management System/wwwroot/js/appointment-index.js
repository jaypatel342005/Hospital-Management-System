'use strict';

document.addEventListener('DOMContentLoaded', function() {
    // Initialize DataTables with server-side processing
    const appointmentsTable = initializeDataTable();
    
    // Initialize filters
    initializeFilters(appointmentsTable);
    
    // Initialize view toggle
    initializeViewToggle();
    
    // Initialize calendar if FullCalendar is loaded
    if (typeof FullCalendar !== 'undefined') {
        initializeCalendar();
    }
});

function initializeDataTable() {
    return new DataTable('#appointmentsTable', {
        responsive: true,
        language: {
            searchPlaceholder: "Search appointments...",
            processing: "Loading appointments...",
            emptyTable: "No appointments found",
        },
        order: [[3, 'desc']], // Sort by date descending
        pageLength: 25,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'collection',
                text: 'Export',
                buttons: ['csv', 'excel', 'pdf']
            }
        ],
        columnDefs: [
            { targets: -1, orderable: false }, // Disable sorting on actions column
            { targets: 3, type: 'date' } // Enable date sorting
        ]
    });
}

function initializeFilters(table) {
    const filters = {
        search: document.getElementById('searchInput'),
        status: document.getElementById('statusFilter'),
        doctor: document.getElementById('doctorFilter'),
        date: document.getElementById('dateFilter')
    };

    // Debounced search function
    const debounce = (fn, delay) => {
        let timeoutId;
        return (...args) => {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => fn.apply(this, args), delay);
        };
    };

    // Apply filters
    const applyFilters = debounce(() => {
        const searchValue = filters.search.value.toLowerCase();
        const statusValue = filters.status.value.toLowerCase();
        const doctorValue = filters.doctor.value;
        const dateValue = filters.date.value;

        table.rows().every(function() {
            const data = this.data();
            const rowVisible = 
                (!searchValue || Object.values(data).some(val => String(val).toLowerCase().includes(searchValue))) &&
                (!statusValue || data[4].toLowerCase().includes(statusValue)) &&
                (!doctorValue || data[2] === doctorValue) &&
                (!dateValue || data[3].includes(dateValue));
            
            this.node().style.display = rowVisible ? '' : 'none';
        });
    }, 300);

    // Add event listeners
    Object.values(filters).forEach(filter => {
        if (filter) {
            filter.addEventListener('input', applyFilters);
            filter.addEventListener('change', applyFilters);
        }
    });
}

function initializeViewToggle() {
    const viewBtns = document.querySelectorAll('.view-toggle button');
    const listView = document.getElementById('listView');
    const calendarView = document.getElementById('calendarView');

    viewBtns.forEach(btn => {
        btn.addEventListener('click', function() {
            const view = this.dataset.view;
            
            // Update button states
            viewBtns.forEach(b => {
                b.classList.remove('active');
                b.setAttribute('aria-pressed', 'false');
            });
            this.classList.add('active');
            this.setAttribute('aria-pressed', 'true');

            // Show selected view
            if (view === 'list') {
                listView.classList.remove('d-none');
                calendarView.classList.add('d-none');
            } else {
                listView.classList.add('d-none');
                calendarView.classList.remove('d-none');
                if (calendar) {
                    calendar.render();
                }
            }
        });
    });
}

let calendar;
function initializeCalendar() {
    const calendarEl = document.getElementById('appointmentCalendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: function(info, successCallback, failureCallback) {
            // Convert table data to calendar events
            const events = [];
            document.querySelectorAll('#appointmentsTable tbody tr').forEach(row => {
                const cells = row.cells;
                if (cells.length >= 7) {
                    const status = cells[4].textContent.trim().toLowerCase();
                    events.push({
                        id: cells[0].textContent,
                        title: `${cells[1].textContent} - Dr. ${cells[2].textContent}`,
                        start: new Date(cells[3].querySelector('time')?.dateTime || cells[3].textContent),
                        className: `fc-event-${status}`,
                        extendedProps: {
                            patient: cells[1].textContent,
                            doctor: cells[2].textContent,
                            status: status,
                            amount: cells[6].textContent
                        }
                    });
                }
            });
            successCallback(events);
        },
        eventClick: function(info) {
            Swal.fire({
                title: info.event.title,
                html: `
                    <div class="text-start">
                        <p><strong>Patient:</strong> ${info.event.extendedProps.patient}</p>
                        <p><strong>Doctor:</strong> ${info.event.extendedProps.doctor}</p>
                        <p><strong>Status:</strong> ${info.event.extendedProps.status}</p>
                        <p><strong>Amount:</strong> ${info.event.extendedProps.amount}</p>
                    </div>
                `,
                showCloseButton: true,
                showCancelButton: true,
                confirmButtonText: 'View Details',
                cancelButtonText: 'Close'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = `/Appointment/Details/${info.event.id}`;
                }
            });
        }
    });
}

async function deleteAppointment(appointmentId) {
    try {
        const result = await Swal.fire({
            title: 'Delete Appointment?',
            text: "This action cannot be undone.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Yes, delete it!'
        });

        if (result.isConfirmed) {
            const response = await fetch(`/Appointment/Delete/${appointmentId}`, {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            
            if (data.success) {
                await Swal.fire(
                    'Deleted!',
                    'The appointment has been deleted.',
                    'success'
                );
                location.reload();
            } else {
                throw new Error(data.message || 'Failed to delete appointment');
            }
        }
    } catch (error) {
        console.error('Error:', error);
        Swal.fire(
            'Error!',
            'Failed to delete the appointment. Please try again.',
            'error'
        );
    }
}

// Handle "Load More" functionality
document.getElementById('loadMoreBtn')?.addEventListener('click', function() {
    const currentRows = document.querySelectorAll('#appointmentsTableBody tr:not(#loadMoreRow)').length;
    fetch(`/Appointment/GetMoreAppointments?skip=${currentRows}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Add new rows to the table
                const tbody = document.getElementById('appointmentsTableBody');
                data.appointments.forEach(appointment => {
                    tbody.insertAdjacentHTML('beforeend', createAppointmentRow(appointment));
                });
                
                // Update or remove "Load More" button
                if (data.hasMore) {
                    document.getElementById('loadMoreBtn').textContent = 
                        `Load More (${data.remainingCount} more)`;
                } else {
                    document.getElementById('loadMoreRow').remove();
                }
            }
        })
        .catch(error => {
            console.error('Error loading more appointments:', error);
            Swal.fire('Error', 'Failed to load more appointments', 'error');
        });
});
