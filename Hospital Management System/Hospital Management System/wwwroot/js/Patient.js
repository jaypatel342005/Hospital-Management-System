
        // Document ready
    window.onload = function () {
        @if (TempData["SuccessMessage"] != null)
    {
        <text>
            Swal.fire({
                icon: 'success',
            title: 'Success',
            text: '@TempData["SuccessMessage"]'
                             });
        </text>
    }

    @if (TempData["ErrorMessage"] != null)
    {
        <text>
            Swal.fire({
                icon: 'error',
            title: 'Error',
            text: '@TempData["ErrorMessage"]'
                             });
        </text>
    }
         };

    function confirmDelete(patientId) {
        Swal.fire({
            title: 'Are you sure?',
            text: "Do you really want to delete this department?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = `/Patient/PatientDelete?PatientID=${patientId}`;
            }
        });
         }



    // Other functions (implement as needed)
    function viewPatient(patientId) {
        window.location.href = `/Patient/PatientDetails?PatientID=${patientId}`;
        }

    function editPatient(patientId) {
        window.location.href = `/Patient/PatientEdit?PatientID=${patientId}`;
        }

    function viewMedicalHistory(patientId) {
        window.location.href = `/Patient/MedicalHistory?PatientID=${patientId}`;
        }

    function bookAppointment(patientId) {
        window.location.href = `/Appointment/BookAppointment?PatientID=${patientId}`;
        }



    // Add this JavaScript code to your existing Patient.js file or in the script section

    // Export functionality - converts table to CSV and downloads
    function exportPatients() {
            const table = document.getElementById('patientsTable');
    if (!table) {
                // If no table (mobile view), create temporary table from card data
                const cards = document.querySelectorAll('.patient-card');
    if (cards.length === 0) {
        alert('No patients to export');
    return;
                }
    exportFromCards(cards);
    return;
            }

    let csv = '';
    const rows = table.querySelectorAll('tr');

            rows.forEach((row, index) => {
                const cols = row.querySelectorAll(index === 0 ? 'th' : 'td');
    const rowData = [];

                cols.forEach((col, colIndex) => {
                    if (colIndex === cols.length - 1) return; // Skip actions column
    let text = col.textContent.trim();
    text = text.replace(/"/g, '""'); // Escape quotes
    rowData.push(`"${text}"`);
                });

    csv += rowData.join(',') + '\n';
            });

    downloadCSV(csv, 'patients.csv');
        }

    // Export from mobile cards view
    function exportFromCards(cards) {
        let csv = 'ID,Name,Gender,Age,Email,Phone,City,Address,State,Status,Registered\n';

            cards.forEach(card => {
                const name = card.querySelector('.card-title').textContent.trim();
    const cardText = card.querySelector('.card-text').textContent;
    const id = cardText.match(/ID:\s*(\d+)/)?.[1] || '';
    const age = cardText.match(/Age:\s*(\d+)/)?.[1] || '';
    const gender = card.querySelector('.badge.bg-primary').textContent.trim();
    const status = card.querySelector('.badge.bg-success, .badge.bg-danger').textContent.trim();

    const contactInfo = card.querySelectorAll('.text-muted.small .col-12');
    const email = contactInfo[0]?.textContent.replace('✉', '').trim() || '';
    const phone = contactInfo[1]?.textContent.replace('📞', '').trim() || '';
    const address = contactInfo[2]?.textContent.replace('🏠', '').trim() || '';
    const location = contactInfo[3]?.textContent.replace('📍', '').trim() || '';
                const [city, state] = location.split(',').map(s => s.trim());
    const registered = contactInfo[4]?.textContent.replace('📅', '').trim() || '';

    csv += `"${id}","${name}","${gender}","${age}","${email}","${phone}","${city || ''}","${address}","${state || ''}","${status}","${registered}"\n`;
            });

    downloadCSV(csv, 'patients.csv');
        }

    // Download CSV file
    function downloadCSV(csv, filename) {
            const blob = new Blob([csv], {type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
        }

    // Filter functionality - works for both desktop table and mobile cards
    function applyFilters() {
            const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const genderFilter = document.getElementById('genderFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;

    // Filter desktop table
    const tableRows = document.querySelectorAll('#patientsTable tbody tr');
            tableRows.forEach(row => {
                const name = row.children[1].textContent.toLowerCase();
    const gender = row.children[2].textContent.trim();
    const email = row.children[4].textContent.toLowerCase();
    const phone = row.children[5].textContent.toLowerCase();
    const city = row.children[6].textContent.toLowerCase();
    const status = row.children[9].textContent.trim();

    const matchesSearch = !searchTerm ||
    name.includes(searchTerm) ||
    email.includes(searchTerm) ||
    phone.includes(searchTerm) ||
    city.includes(searchTerm);

    const matchesGender = !genderFilter || gender === genderFilter;
    const matchesStatus = !statusFilter || status === statusFilter;

    row.style.display = matchesSearch && matchesGender && matchesStatus ? '' : 'none';
            });

    // Filter mobile cards
    const cards = document.querySelectorAll('.patient-card');
            cards.forEach(card => {
                const name = card.querySelector('.card-title').textContent.toLowerCase();
    const gender = card.querySelector('.badge.bg-primary').textContent.trim();
    const status = card.querySelector('.badge.bg-success, .badge.bg-danger').textContent.trim();
    const cardText = card.textContent.toLowerCase();

    const matchesSearch = !searchTerm || cardText.includes(searchTerm);
    const matchesGender = !genderFilter || gender === genderFilter;
    const matchesStatus = !statusFilter || status === statusFilter;

    card.parentElement.style.display = matchesSearch && matchesGender && matchesStatus ? '' : 'none';
            });
        }

    // Clear all filters
    function clearFilters() {
        document.getElementById('searchInput').value = '';
    document.getElementById('genderFilter').value = '';
    document.getElementById('statusFilter').value = '';
    applyFilters();
        }

    // Initialize filters on page load
    document.addEventListener('DOMContentLoaded', function() {
            // Add event listeners for real-time filtering
            const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', applyFilters);
            }

    const genderFilter = document.getElementById('genderFilter');
    if (genderFilter) {
        genderFilter.addEventListener('change', applyFilters);
            }

    const statusFilter = document.getElementById('statusFilter');
    if (statusFilter) {
        statusFilter.addEventListener('change', applyFilters);
            }
        });


