
        // Global variables
    let calendar;
    let appointmentsData = [];

        // Convert server data to JavaScript format
        @if (Model != null && Model.Rows.Count > 0)
    {
        <text>
            appointmentsData = [
            @foreach (DataRow row in Model.Rows)
            {
                            var appointmentDate = row["AppointmentDate"];
            var dateString = "";
            if (appointmentDate != null && DateTime.TryParse(appointmentDate.ToString(), out DateTime parsedDate))
            {
                dateString = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                            }

            <text>
                {
                    id: '@row["AppointmentID"]',
                title: '@Html.Raw(Json.Serialize($"{row["PatientName"]} - {row["DoctorName"]}"))',
                start: '@dateString',
                className: '@(row["AppointmentStatus"]?.ToString()?.ToLower() ?? "scheduled")',
                patientName: '@Html.Raw(Json.Serialize(row["PatientName"]?.ToString() ?? ""))',
                doctorName: '@Html.Raw(Json.Serialize(row["DoctorName"]?.ToString() ?? ""))',
                status: '@Html.Raw(Json.Serialize(row["AppointmentStatus"]?.ToString() ?? ""))',
                description: '@Html.Raw(Json.Serialize(row["Description"]?.ToString() ?? ""))',
                amount: '@(row["TotalConsultedAmount"]?.ToString() ?? "0")'
                            }@(Model.Rows.IndexOf(row) == Model.Rows.Count - 1 ? "" : ",")
            </text>
                    }
            ];
        </text>
    }

        // Initialize calendar
    document.addEventListener('DOMContentLoaded', function() {
        console.log('Initializing calendar with data:', appointmentsData);
    initializeCalendar();
    setupEventListeners();
        });

    function initializeCalendar() {
            var calendarEl = document.getElementById('calendar');

    if (!calendarEl) {
        console.error('Calendar element not found');
    return;
            }

            // Filter out events with invalid dates
            const validEvents = appointmentsData.filter(appointment => {
                return appointment.start && appointment.start !== '';
            });

    console.log('Valid events for calendar:', validEvents);

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
    height: 'auto',
    headerToolbar: {
        left: 'prev,next today',
    center: 'title',
    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
    events: validEvents,
    eventClick: function(info) {
        showAppointmentDetails(info.event.id);
                },
    eventDidMount: function(info) {
                    // Add custom styling based on status
                    const status = info.event.extendedProps.status || info.event.classNames[0];
    const element = info.el;

    // Remove default styling and add custom
    element.style.border = 'none';
    element.style.borderRadius = '6px';
    element.style.padding = '2px 6px';
    element.style.fontSize = '0.875rem';
    element.style.fontWeight = '500';

    switch(status?.toLowerCase()) {
                        case 'completed':
    element.style.backgroundColor = '#198754';
    element.style.color = 'white';
    break;
    case 'pending':
    element.style.backgroundColor = '#ffc107';
    element.style.color = 'black';
    break;
    case 'cancelled':
    element.style.backgroundColor = '#dc3545';
    element.style.color = 'white';
    break;
    default: // scheduled
    element.style.backgroundColor = '#4154f1';
    element.style.color = 'white';
                    }
                },
    eventMouseEnter: function(info) {
                    // Show tooltip on hover
                    const tooltip = document.createElement('div');
    tooltip.className = 'calendar-tooltip';
    tooltip.innerHTML = `
    <strong>${info.event.extendedProps.patientName}</strong><br>
        <strong>Doctor:</strong> ${info.event.extendedProps.doctorName}<br>
            <strong>Status:</strong> ${info.event.extendedProps.status}<br>
                <strong>Time:</strong> ${info.event.start.toLocaleTimeString()}<br>
                    ${info.event.extendedProps.description ? `<strong>Description:</strong> ${info.event.extendedProps.description}<br>` : ''}
                    ${info.event.extendedProps.amount ? `<strong>Amount:</strong> ${info.event.extendedProps.amount}` : ''}
                    `;
                    tooltip.style.cssText = `
                    position: absolute;
                    background: rgba(0,0,0,0.8);
                    color: white;
                    padding: 8px 12px;
                    border-radius: 4px;
                    font-size: 12px;
                    z-index: 1000;
                    max-width: 250px;
                    box-shadow: 0 2px 10px rgba(0,0,0,0.2);
                    `;

                    document.body.appendChild(tooltip);

                    const rect = info.el.getBoundingClientRect();
                    tooltip.style.left = rect.left + 'px';
                    tooltip.style.top = (rect.bottom + 5) + 'px';

                    info.el.tooltip = tooltip;
                },
                    eventMouseLeave: function(info) {
                    if (info.el.tooltip) {
                        document.body.removeChild(info.el.tooltip);
                    info.el.tooltip = null;
                    }
                }
            });

                    try {
                        calendar.render();
                    console.log('Calendar rendered successfully');
            } catch (error) {
                        console.error('Error rendering calendar:', error);
            }
        }

                    function setupEventListeners() {
                        // Search functionality
                        document.getElementById('searchInput').addEventListener('input', function () {
                            filterTable();
                        });

                    // Filter functionality
                    document.getElementById('statusFilter').addEventListener('change', function() {
                        filterTable();
            });

                    document.getElementById('doctorFilter').addEventListener('change', function() {
                        filterTable();
            });

                    document.getElementById('dateFilter').addEventListener('change', function() {
                        filterTable();
            });
        }

                    function switchView(viewType) {
            const listView = document.querySelector('.list-view');
                    const calendarView = document.querySelector('.calendar-view');
                    const listBtn = document.querySelector('.view-toggle button:first-child');
                    const calendarBtn = document.querySelector('.view-toggle button:last-child');

                    if (viewType === 'list') {
                        listView.classList.add('active');
                    calendarView.classList.remove('active');
                    listBtn.classList.add('active');
                    calendarBtn.classList.remove('active');
            } else {
                        listView.classList.remove('active');
                    calendarView.classList.add('active');
                    listBtn.classList.remove('active');
                    calendarBtn.classList.add('active');

                    // Initialize calendar if it doesn't exist, otherwise refresh it
                    if (!calendar) {
                        setTimeout(() => {
                            initializeCalendar();
                        }, 100);
                } else {
                        setTimeout(() => {
                            calendar.render();
                            calendar.updateSize();
                        }, 100);
                }
            }
        }

                    function filterTable() {
            const searchTerm = document.getElementById('searchInput').value.toLowerCase();
                    const statusFilter = document.getElementById('statusFilter').value.toLowerCase();
                    const doctorFilter = document.getElementById('doctorFilter').value;
                    const dateFilter = document.getElementById('dateFilter').value;

                    const tbody = document.getElementById('appointmentsTableBody');
                    const rows = tbody.querySelectorAll('tr');

            rows.forEach(row => {
                if (row.cells.length === 1) return; // Skip "no data" row

                    const patientName = row.cells[1].textContent.toLowerCase();
                    const doctorName = row.cells[2].textContent.toLowerCase();
                    const appointmentDate = row.cells[3].textContent;
                    const status = row.cells[4].textContent.toLowerCase();
                    const description = row.cells[5].textContent.toLowerCase();

                    let showRow = true;

                    // Search filter
                    if (searchTerm) {
                        showRow = showRow && (
                            patientName.includes(searchTerm) ||
                            doctorName.includes(searchTerm) ||
                            description.includes(searchTerm)
                        );
                }

                    // Status filter
                    if (statusFilter && showRow) {
                        showRow = status.includes(statusFilter);
                }

                    // Doctor filter
                    if (doctorFilter && showRow) {
                        // This would need to be implemented based on your doctor ID logic
                        // For now, we'll filter by doctor name
                        showRow = doctorName === doctorFilter.toLowerCase();
                }

                    // Date filter
                    if (dateFilter && showRow) {
                    const filterDate = new Date(dateFilter);
                    const appointmentDateObj = new Date(appointmentDate);
                    showRow = appointmentDateObj.toDateString() === filterDate.toDateString();
                }

                    row.style.display = showRow ? '' : 'none';
            });
        }

                    function showAppointmentDetails(appointmentId) {
                        // Redirect to details page
                        window.location.href = '@Url.Action("Details", "Appointment")' + '/' + appointmentId;
        }

                    function deleteAppointment(appointmentId) {

                    }

                    function exportData() {
            // Simple CSV export
            const table = document.getElementById('appointmentsTable');
                    const rows = Array.from(table.querySelectorAll('tr'));
            const csvContent = rows.map(row => {
                const cells = Array.from(row.querySelectorAll('th, td'));
                return cells.slice(0, -1).map(cell => `"${cell.textContent.trim()}"`).join(',');
            }).join('\n');

                    const blob = new Blob([csvContent], {type: 'text/csv' });
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = 'appointments_' + new Date().toISOString().split('T')[0] + '.csv';
                    a.click();
                    window.URL.revokeObjectURL(url);
        }
