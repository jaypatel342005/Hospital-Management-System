console.log("JavaScript.js loaded successfully");


// Complete CSV export functionality
function exportTableToCSV(tableId, filename) {
    const table = document.getElementById(tableId);
    const rows = table.querySelectorAll('tr');
    const csvContent = [];

    // Process each row
    rows.forEach(row => {
        const cols = row.querySelectorAll('td, th');
        const rowData = [];

        cols.forEach(col => {
            // Clean the text and handle commas/quotes
            let cellText = col.textContent.trim();
            // Escape quotes by doubling them and wrap in quotes if contains comma/quote
            if (cellText.includes(',') || cellText.includes('"') || cellText.includes('\n')) {
                cellText = '"' + cellText.replace(/"/g, '""') + '"';
            }
            rowData.push(cellText);
        });

        csvContent.push(rowData.join(','));
    });

    // Create and download the CSV
    const csvString = csvContent.join('\n');
    downloadCSV(csvString, filename);
}

// Missing downloadCSV function - this was causing the error
function downloadCSV(csvContent, filename) {
    // Create a Blob with the CSV content
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });

    // Create a link element for download
    const link = document.createElement('a');

    if (link.download !== undefined) {
        // Create a URL for the blob
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', filename);
        link.style.visibility = 'hidden';

        // Add link to DOM, click it, then remove it
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        // Clean up the URL object
        URL.revokeObjectURL(url);
    }
}

// DataTable initialization
import { DataTable } from "../dist/module.js"
new DataTable("#demo-table", {
    paging: false,
    scrollY: "30vh",
    rowNavigation: true,
    tabIndex: 1
})

let patientToDelete = null;

// Initialize page
document.addEventListener('DOMContentLoaded', function () {
    populateCityFilter();
    updateCounts();

    // Initialize DataTable for desktop view
    if (window.innerWidth >= 992) {
        $('#patientsTable').DataTable({
            responsive: true,
            pageLength: 10,
            order: [[0, 'desc']],
            columnDefs: [
                { orderable: false, targets: [10] }
            ]
        });
    }
});

// Calculate age from DOB
function calculateAge(dob) {
    const today = new Date();
    const birthDate = new Date(dob);
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }

    return age;
}

// Populate city filter dropdown
function populateCityFilter() {
    const cityFilter = document.getElementById('cityFilter');
    const cities = new Set();

    // Extract cities from table rows
    const rows = document.querySelectorAll('#patientsTable tbody tr, #mobileCards .card');
    rows.forEach(row => {
        let city;
        if (row.tagName === 'TR') {
            city = row.cells[6].textContent.trim();
        } else {
            const cityText = row.querySelector('.bi-geo-alt').parentElement.textContent;
            city = cityText.split(',')[0].trim();
        }
        if (city) cities.add(city);
    });

    cities.forEach(city => {
        const option = document.createElement('option');
        option.value = city;
        option.textContent = city;
        cityFilter.appendChild(option);
    });
}

// Apply filters
function applyFilters() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const genderFilter = document.getElementById('genderFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;
    const cityFilter = document.getElementById('cityFilter').value;
    const ageFilter = document.getElementById('ageFilter').value;

    // Filter desktop table
    const tableRows = document.querySelectorAll('#patientsTable tbody tr');
    const mobileCards = document.querySelectorAll('#mobileCards .col-12');

    let visibleCount = 0;

    // Filter desktop table
    tableRows.forEach(row => {
        const name = row.cells[1].textContent.toLowerCase();
        const email = row.cells[4].textContent.toLowerCase();
        const phone = row.cells[5].textContent.toLowerCase();
        const gender = row.cells[2].textContent.trim();
        const age = parseInt(row.cells[3].textContent.trim());
        const city = row.cells[6].textContent.trim();
        const status = row.cells[8].textContent.trim();

        const searchMatch = !searchInput ||
            name.includes(searchInput) ||
            email.includes(searchInput) ||
            phone.includes(searchInput);
        const genderMatch = !genderFilter || gender === genderFilter;
        const statusMatch = !statusFilter || status === statusFilter;
        const cityMatch = !cityFilter || city === cityFilter;
        const ageMatch = !ageFilter || checkAgeRange(age, ageFilter);

        const show = searchMatch && genderMatch && statusMatch && cityMatch && ageMatch;
        row.style.display = show ? '' : 'none';
        if (show) visibleCount++;
    });

    // Filter mobile cards
    mobileCards.forEach(card => {
        const cardText = card.textContent.toLowerCase();
        const badge = card.querySelector('.badge');
        const status = badge.textContent.trim();
        const genderBadge = card.querySelector('.badge.bg-primary');
        const gender = genderBadge ? genderBadge.textContent.trim() : '';

        const searchMatch = !searchInput || cardText.includes(searchInput);
        const genderMatch = !genderFilter || gender === genderFilter;
        const statusMatch = !statusFilter || status === statusFilter;
        // Add more specific matching for mobile cards as needed

        const show = searchMatch && genderMatch && statusMatch;
        card.style.display = show ? '' : 'none';
    });

    updateCounts();
}

// Check age range
function checkAgeRange(age, range) {
    switch (range) {
        case '0-18': return age >= 0 && age <= 18;
        case '19-30': return age >= 19 && age <= 30;
        case '31-50': return age >= 31 && age <= 50;
        case '51-70': return age >= 51 && age <= 70;
        case '70+': return age > 70;
        default: return true;
    }
}

// Clear all filters
function clearFilters() {
    document.getElementById('searchInput').value = '';
    document.getElementById('genderFilter').value = '';
    document.getElementById('statusFilter').value = '';
    document.getElementById('cityFilter').value = '';
    document.getElementById('ageFilter').value = '';
    applyFilters();
}

// Update counts
function updateCounts() {
    const visibleRows = document.querySelectorAll('#patientsTable tbody tr:not([style*="display: none"])');
    const visibleCards = document.querySelectorAll('#mobileCards .col-12:not([style*="display: none"])');

    const visibleCount = Math.max(visibleRows.length, visibleCards.length);
    document.getElementById('visibleCount').textContent = visibleCount;
}

// Confirm delete
function confirmDelete(patientId) {
    patientToDelete = patientId;
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
}

// Delete patient
document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
    if (patientToDelete) {
        // Add your delete logic here
        console.log('Deleting patient:', patientToDelete);
        // For now, just hide the modal
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
    }
});

// Action functions
function viewPatient(patientId) {
    console.log('View patient:', patientId);
    // Implement view patient functionality
}

function editPatient(patientId) {
    console.log('Edit patient:', patientId);
    // Implement edit patient functionality
}

function viewMedicalHistory(patientId) {
    console.log('View medical history:', patientId);
    // Implement medical history view
}

function bookAppointment(patientId) {
    console.log('Book appointment for patient:', patientId);
    // Implement appointment booking
}

// CORRECTED: Export patients function - now actually exports instead of just logging
function exportPatients() {
    // Check if we have visible (filtered) data to export
    const visibleRows = document.querySelectorAll('#patientsTable tbody tr:not([style*="display: none"])');

    if (visibleRows.length === 0) {
        alert('No patients to export. Please check your filters.');
        return;
    }

    // Export only visible patients (respecting current filters)
    exportFilteredPatientsToCSV();
}

// New function to export only filtered/visible patients
function exportFilteredPatientsToCSV() {
    const table = document.getElementById('patientsTable');
    const headerRow = table.querySelector('thead tr');
    const visibleRows = table.querySelectorAll('tbody tr:not([style*="display: none"])');

    const csvContent = [];

    // Add header row
    const headerCols = headerRow.querySelectorAll('th');
    const headerData = [];
    headerCols.forEach(col => {
        let headerText = col.textContent.trim();
        if (headerText.includes(',') || headerText.includes('"') || headerText.includes('\n')) {
            headerText = '"' + headerText.replace(/"/g, '""') + '"';
        }
        headerData.push(headerText);
    });
    csvContent.push(headerData.join(','));

    // Add visible data rows
    visibleRows.forEach(row => {
        const cols = row.querySelectorAll('td');
        const rowData = [];

        cols.forEach(col => {
            let cellText = col.textContent.trim();
            if (cellText.includes(',') || cellText.includes('"') || cellText.includes('\n')) {
                cellText = '"' + cellText.replace(/"/g, '""') + '"';
            }
            rowData.push(cellText);
        });

        csvContent.push(rowData.join(','));
    });

    // Create filename with timestamp
    const now = new Date();
    const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-');
    const filename = `patients_export_${timestamp}.csv`;

    // Download the CSV
    const csvString = csvContent.join('\n');
    downloadCSV(csvString, filename);
}

// Add patient form submission
document.getElementById('addPatientForm').addEventListener('submit', function (e) {
    e.preventDefault();
    console.log('Add new patient form submitted');
    // Implement add patient functionality
    // Close modal after successful submission
    bootstrap.Modal.getInstance(document.getElementById('addPatientModal')).hide();
});