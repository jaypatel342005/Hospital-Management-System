/**
 * Hospital Management System - Dashboard JavaScript
 * Handles Chart.js initialization, AJAX calls, and dashboard interactions
 */

const DashboardManager = {
    charts: {},
    currentRange: 'month',
    debounceTimer: null,
    
    // Chart.js global configuration
    defaultChartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'top',
            },
            tooltip: {
                enabled: true,
                backgroundColor: 'rgba(0,0,0,0.8)',
                titleColor: 'white',
                bodyColor: 'white',
                borderColor: 'rgba(255,255,255,0.1)',
                borderWidth: 1
            }
        },
        animation: {
            duration: 750,
            easing: 'easeInOutQuart'
        }
    },

    init() {
        console.log('Dashboard Manager initializing...');
        this.setupEventListeners();
        this.initializeCharts();
        this.loadInitialData();
    },

    setupEventListeners() {
        // Date range filter
        const dateRangeFilter = document.getElementById('dateRangeFilter');
        if (dateRangeFilter) {
            dateRangeFilter.addEventListener('change', (e) => {
                this.currentRange = e.target.value;
                this.debounceFilterChange();
            });
        }

        // Refresh button
        const refreshButton = document.getElementById('refreshButton');
        if (refreshButton) {
            refreshButton.addEventListener('click', () => {
                this.refreshAllData();
            });
        }

        // Tab change events for chart initialization
        const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');
        tabButtons.forEach(button => {
            button.addEventListener('shown.bs.tab', (e) => {
                const targetTab = e.target.getAttribute('data-bs-target');
                this.handleTabChange(targetTab);
            });
        });

        // Carousel slide events for chart resize
        const carousel = document.getElementById('overviewCarousel');
        if (carousel) {
            carousel.addEventListener('slid.bs.carousel', () => {
                this.handleCarouselSlide();
            });
        }

        // KPI card click events (navigate to respective modules)
        this.setupKPICardClicks();
    },

    setupKPICardClicks() {
        const patientsCard = document.querySelector('.patients-card');
        const doctorsCard = document.querySelector('.doctors-card');
        const appointmentsCard = document.querySelector('.appointments-card');
        const revenueCard = document.querySelector('.revenue-card');

        if (patientsCard) {
            patientsCard.addEventListener('click', () => {
                window.location.href = '/Patients';
            });
        }

        if (doctorsCard) {
            doctorsCard.addEventListener('click', () => {
                window.location.href = '/Doctors';
            });
        }

        if (appointmentsCard) {
            appointmentsCard.addEventListener('click', () => {
                window.location.href = '/Appointments';
            });
        }

        if (revenueCard) {
            revenueCard.addEventListener('click', () => {
                window.location.href = '/Billing';
            });
        }
    },

    initializeCharts() {
        // Overview tab charts (carousel)
        this.initGenderPieChart();
        this.initAppointmentStatusChart();
        this.initDepartmentDoctorsChart();
        
        // Other tab charts will be initialized when tabs are shown
    },

    loadInitialData() {
        // Load data for the overview tab (initially active)
        this.loadGenderDistribution();
        this.loadAppointmentStatus();
        this.loadDepartmentDoctors();
    },

    debounceFilterChange() {
        clearTimeout(this.debounceTimer);
        this.debounceTimer = setTimeout(() => {
            this.refreshAllData();
        }, 500);
    },

    async refreshAllData() {
        this.showGlobalLoading(true);
        
        try {
            // Update KPIs
            await this.updateKPIs();
            
            // Update charts based on current active tab
            const activeTab = document.querySelector('.tab-pane.active');
            if (activeTab) {
                const tabId = activeTab.getAttribute('id');
                await this.updateTabCharts(tabId);
            }
            
            this.showToast('Dashboard updated successfully', 'success');
        } catch (error) {
            console.error('Error refreshing dashboard:', error);
            this.showToast('Error updating dashboard', 'danger');
        } finally {
            this.showGlobalLoading(false);
        }
    },

    async updateKPIs() {
        // In a real implementation, you would make AJAX calls to update KPIs
        // For now, we'll just reload the page with the new date range
        const currentUrl = new URL(window.location);
        currentUrl.searchParams.set('dateRange', this.currentRange);
        
        // Update URL without reloading
        window.history.pushState({}, '', currentUrl);
        
        // You could implement AJAX calls here to update KPIs without page reload
        console.log('KPIs would be updated via AJAX for range:', this.currentRange);
    },

    handleTabChange(targetTab) {
        setTimeout(() => {
            switch (targetTab) {
                case '#appointments':
                    this.initTopDoctorsChart();
                    this.initAppointmentTypesRadarChart();
                    this.loadTopDoctors();
                    this.loadAppointmentTypesRadar();
                    break;
                case '#finance':
                    this.initRevenueMonthlyChart();
                    this.initRevenueVsAppointmentsChart();
                    this.loadRevenueMonthly();
                    this.loadRevenueVsAppointments();
                    break;
                case '#people':
                    this.initPatientsMonthlyChart();
                    this.initDepartmentDoctorsTabChart();
                    this.loadPatientsMonthly();
                    this.loadDepartmentDoctorsTab();
                    break;
                case '#overview':
                    // Overview charts are already initialized
                    this.loadGenderDistribution();
                    this.loadAppointmentStatus();
                    this.loadDepartmentDoctors();
                    break;
            }
        }, 100); // Small delay to ensure tab content is visible
    },

    handleCarouselSlide() {
        // Resize charts when carousel slides change
        setTimeout(() => {
            Object.values(this.charts).forEach(chart => {
                if (chart && typeof chart.resize === 'function') {
                    chart.resize();
                }
            });
        }, 300);
    },

    async updateTabCharts(tabId) {
        switch (tabId) {
            case 'overview':
                await Promise.all([
                    this.loadGenderDistribution(),
                    this.loadAppointmentStatus(),
                    this.loadDepartmentDoctors()
                ]);
                break;
            case 'appointments':
                await Promise.all([
                    this.loadTopDoctors(),
                    this.loadAppointmentTypesRadar()
                ]);
                break;
            case 'finance':
                await Promise.all([
                    this.loadRevenueMonthly(),
                    this.loadRevenueVsAppointments()
                ]);
                break;
            case 'people':
                await Promise.all([
                    this.loadPatientsMonthly(),
                    this.loadDepartmentDoctorsTab()
                ]);
                break;
        }
    },

    // Chart Initialization Methods
    initGenderPieChart() {
        const ctx = document.getElementById('genderPieChart');
        if (!ctx) return;

        this.charts.genderPie = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: [],
                datasets: [{
                    data: [],
                    backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                ...this.defaultChartOptions,
                plugins: {
                    ...this.defaultChartOptions.plugins,
                    title: {
                        display: true,
                        text: 'Doctors by Department'
                    }
                }
            }
        });
    },

    initTopDoctorsChart() {
        const ctx = document.getElementById('topDoctorsChart');
        if (!ctx || this.charts.topDoctors) return;

        this.charts.topDoctors = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [{
                    label: 'Appointments',
                    data: [],
                    backgroundColor: 'rgba(255, 99, 132, 0.8)',
                    borderColor: '#FF6384',
                    borderWidth: 1
                }]
            },
            options: {
                ...this.defaultChartOptions,
                indexAxis: 'y',
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(0,0,0,0.1)'
                        }
                    },
                    y: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    },

    initAppointmentTypesRadarChart() {
        const ctx = document.getElementById('appointmentTypesRadarChart');
        if (!ctx || this.charts.appointmentTypesRadar) return;

        this.charts.appointmentTypesRadar = new Chart(ctx, {
            type: 'radar',
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Scheduled',
                        data: [],
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: '#36A2EB',
                        borderWidth: 2
                    },
                    {
                        label: 'Completed',
                        data: [],
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: '#4BC0C0',
                        borderWidth: 2
                    },
                    {
                        label: 'Cancelled',
                        data: [],
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        borderColor: '#FF6384',
                        borderWidth: 2
                    }
                ]
            },
            options: {
                ...this.defaultChartOptions,
                scales: {
                    r: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(0,0,0,0.1)'
                        }
                    }
                }
            }
        });
    },

    initRevenueMonthlyChart() {
        const ctx = document.getElementById('revenueMonthlyChart');
        if (!ctx || this.charts.revenueMonthly) return;

        this.charts.revenueMonthly = new Chart(ctx, {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Revenue',
                    data: [],
                    borderColor: '#36A2EB',
                    backgroundColor: 'rgba(54, 162, 235, 0.1)',
                    tension: 0.4,
                    fill: true,
                    borderWidth: 3
                }]
            },
            options: {
                ...this.defaultChartOptions,
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(0,0,0,0.1)'
                        },
                        ticks: {
                            callback: function(value) {
                                return '₹' + value.toLocaleString();
                            }
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    },

    initRevenueVsAppointmentsChart() {
        const ctx = document.getElementById('revenueVsAppointmentsChart');
        if (!ctx || this.charts.revenueVsAppointments) return;

        this.charts.revenueVsAppointments = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Appointments',
                        data: [],
                        backgroundColor: 'rgba(54, 162, 235, 0.6)',
                        borderColor: '#36A2EB',
                        borderWidth: 1,
                        yAxisID: 'y'
                    },
                    {
                        label: 'Revenue',
                        data: [],
                        backgroundColor: 'rgba(255, 99, 132, 0.6)',
                        borderColor: '#FF6384',
                        borderWidth: 1,
                        yAxisID: 'y1'
                    }
                ]
            },
            options: {
                ...this.defaultChartOptions,
                scales: {
                    y: {
                        type: 'linear',
                        display: true,
                        position: 'left',
                        beginAtZero: true
                    },
                    y1: {
                        type: 'linear',
                        display: true,
                        position: 'right',
                        beginAtZero: true,
                        grid: {
                            drawOnChartArea: false,
                        },
                        ticks: {
                            callback: function(value) {
                                return '₹' + value.toLocaleString();
                            }
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    },

    initPatientsMonthlyChart() {
        const ctx = document.getElementById('patientsMonthlyChart');
        if (!ctx || this.charts.patientsMonthly) return;

        this.charts.patientsMonthly = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [{
                    label: 'New Patients',
                    data: [],
                    backgroundColor: 'rgba(75, 192, 192, 0.6)',
                    borderColor: '#4BC0C0',
                    borderWidth: 1
                }]
            },
            options: {
                ...this.defaultChartOptions,
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(0,0,0,0.1)'
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    },

    initDepartmentDoctorsTabChart() {
        const ctx = document.getElementById('departmentDoctorsTabChart');
        if (!ctx || this.charts.departmentDoctorsTab) return;

        this.charts.departmentDoctorsTab = new Chart(ctx, {
            type: 'polarArea',
            data: {
                labels: [],
                datasets: [{
                    data: [],
                    backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40'],
                    borderWidth: 1,
                    borderColor: '#fff'
                }]
            },
            options: {
                ...this.defaultChartOptions,
                plugins: {
                    ...this.defaultChartOptions.plugins,
                    title: {
                        display: true,
                        text: 'Department-wise Doctor Distribution'
                    }
                }
            }
        });
    },

    // Data Loading Methods
    async loadGenderDistribution() {
        this.showChartLoading('genderPieChart', true);
        try {
            const response = await fetch(`/Dashboard/GenderBreakdown?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.genderPie && data.labels) {
                this.charts.genderPie.data.labels = data.labels;
                this.charts.genderPie.data.datasets[0].data = data.datasets[0].data;
                this.charts.genderPie.update('active');
            }
        } catch (error) {
            console.error('Error loading gender distribution:', error);
        } finally {
            this.showChartLoading('genderPieChart', false);
        }
    },

    async loadAppointmentStatus() {
        this.showChartLoading('appointmentStatusChart', true);
        try {
            const response = await fetch(`/Dashboard/AppointmentStatus?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.appointmentStatus && data.labels) {
                this.charts.appointmentStatus.data.labels = data.labels;
                this.charts.appointmentStatus.data.datasets[0].data = data.datasets[0].data;
                this.charts.appointmentStatus.update('active');
            }
        } catch (error) {
            console.error('Error loading appointment status:', error);
        } finally {
            this.showChartLoading('appointmentStatusChart', false);
        }
    },

    async loadDepartmentDoctors() {
        this.showChartLoading('departmentDoctorsChart', true);
        try {
            const response = await fetch(`/Dashboard/DepartmentDoctorCount?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.departmentDoctors && data.labels) {
                this.charts.departmentDoctors.data.labels = data.labels;
                this.charts.departmentDoctors.data.datasets[0].data = data.datasets[0].data;
                this.charts.departmentDoctors.update('active');
            }
        } catch (error) {
            console.error('Error loading department doctors:', error);
        } finally {
            this.showChartLoading('departmentDoctorsChart', false);
        }
    },

    async loadTopDoctors() {
        this.showChartLoading('topDoctorsChart', true);
        try {
            const response = await fetch(`/Dashboard/TopDoctors?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.topDoctors && data.labels) {
                this.charts.topDoctors.data.labels = data.labels;
                this.charts.topDoctors.data.datasets[0].data = data.datasets[0].data;
                this.charts.topDoctors.update('active');
            }
        } catch (error) {
            console.error('Error loading top doctors:', error);
        } finally {
            this.showChartLoading('topDoctorsChart', false);
        }
    },

    async loadAppointmentTypesRadar() {
        this.showChartLoading('appointmentTypesRadarChart', true);
        try {
            const response = await fetch(`/Dashboard/AppointmentTypesRadar?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.appointmentTypesRadar && data.labels) {
                this.charts.appointmentTypesRadar.data.labels = data.labels;
                this.charts.appointmentTypesRadar.data.datasets = data.datasets;
                this.charts.appointmentTypesRadar.update('active');
            }
        } catch (error) {
            console.error('Error loading appointment types radar:', error);
        } finally {
            this.showChartLoading('appointmentTypesRadarChart', false);
        }
    },

    async loadRevenueMonthly() {
        this.showChartLoading('revenueMonthlyChart', true);
        try {
            const response = await fetch(`/Dashboard/RevenueMonthly?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.revenueMonthly && data.labels) {
                this.charts.revenueMonthly.data.labels = data.labels;
                this.charts.revenueMonthly.data.datasets[0].data = data.datasets[0].data;
                this.charts.revenueMonthly.update('active');
            }
        } catch (error) {
            console.error('Error loading monthly revenue:', error);
        } finally {
            this.showChartLoading('revenueMonthlyChart', false);
        }
    },

    async loadRevenueVsAppointments() {
        this.showChartLoading('revenueVsAppointmentsChart', true);
        try {
            const response = await fetch(`/Dashboard/RevenueVsAppointments?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.revenueVsAppointments && data.labels) {
                this.charts.revenueVsAppointments.data.labels = data.labels;
                this.charts.revenueVsAppointments.data.datasets = data.datasets;
                this.charts.revenueVsAppointments.update('active');
            }
        } catch (error) {
            console.error('Error loading revenue vs appointments:', error);
        } finally {
            this.showChartLoading('revenueVsAppointmentsChart', false);
        }
    },

    async loadPatientsMonthly() {
        this.showChartLoading('patientsMonthlyChart', true);
        try {
            const response = await fetch(`/Dashboard/PatientsMonthly?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.patientsMonthly && data.labels) {
                this.charts.patientsMonthly.data.labels = data.labels;
                this.charts.patientsMonthly.data.datasets[0].data = data.datasets[0].data;
                this.charts.patientsMonthly.update('active');
            }
        } catch (error) {
            console.error('Error loading monthly patients:', error);
        } finally {
            this.showChartLoading('patientsMonthlyChart', false);
        }
    },

    async loadDepartmentDoctorsTab() {
        this.showChartLoading('departmentDoctorsTabChart', true);
        try {
            const response = await fetch(`/Dashboard/DepartmentDoctorCount?range=${this.currentRange}`);
            const data = await response.json();
            
            if (this.charts.departmentDoctorsTab && data.labels) {
                this.charts.departmentDoctorsTab.data.labels = data.labels;
                this.charts.departmentDoctorsTab.data.datasets[0].data = data.datasets[0].data;
                this.charts.departmentDoctorsTab.update('active');
            }
        } catch (error) {
            console.error('Error loading department doctors (tab):', error);
        } finally {
            this.showChartLoading('departmentDoctorsTabChart', false);
        }
    },

    // Utility Methods
    showChartLoading(chartId, show) {
        const chartContainer = document.getElementById(chartId)?.closest('div[style*="height"]');
        if (!chartContainer) return;
        
        const loadingElement = chartContainer.querySelector('.chart-loading');
        if (loadingElement) {
            loadingElement.classList.toggle('d-none', !show);
        }
    },

    showGlobalLoading(show) {
        const loadingElements = document.querySelectorAll('.loading-spinner');
        loadingElements.forEach(el => {
            el.classList.toggle('d-none', !show);
        });
    },

    showToast(message, type = 'info') {
        // Simple toast notification - you can enhance this with a proper toast library
        const toast = document.createElement('div');
        toast.className = `alert alert-${type} position-fixed top-0 end-0 m-3`;
        toast.style.zIndex = '9999';
        toast.style.maxWidth = '300px';
        toast.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        document.body.appendChild(toast);
        
        // Auto remove after 3 seconds
        setTimeout(() => {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 3000);
    },

    // Export functionality helpers
    exportTableToExcel(tableId, filename = 'appointments') {
        // Simple Excel export using HTML table
        // For production, consider using libraries like SheetJS
        const table = document.getElementById(tableId);
        if (!table) return;
        
        let csv = '';
        const rows = table.querySelectorAll('tr');
        
        rows.forEach(row => {
            const cols = row.querySelectorAll('td, th');
            const csvRow = Array.from(cols).map(col => {
                let cellData = col.textContent.trim();
                // Handle commas and quotes in data
                if (cellData.includes(',') || cellData.includes('"') || cellData.includes('\n')) {
                    cellData = '"' + cellData.replace(/"/g, '""') + '"';
                }
                return cellData;
            }).join(',');
            csv += csvRow + '\n';
        });
        
        // Create download link
        const blob = new Blob([csv], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.setAttribute('hidden', '');
        a.setAttribute('href', url);
        a.setAttribute('download', filename + '.csv');
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    },

    exportTableToPDF(tableId, filename = 'appointments') {
        // Simple PDF export - for production, consider using jsPDF
        console.log('PDF export functionality - implement with jsPDF library');
        this.showToast('PDF export feature coming soon!', 'info');
    },

    // Cleanup method
    destroy() {
        Object.values(this.charts).forEach(chart => {
            if (chart && typeof chart.destroy === 'function') {
                chart.destroy();
            }
        });
        this.charts = {};
        
        if (this.debounceTimer) {
            clearTimeout(this.debounceTimer);
        }
    }
};

// Initialize when DOM is loaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => DashboardManager.init());
} else {
    DashboardManager.init();
}

// Export for global access
window.DashboardManager = DashboardManager;'
                }]
            },
            options: {
                ...this.defaultChartOptions,
                plugins: {
                    ...this.defaultChartOptions.plugins,
                    title: {
                        display: true,
                        text: 'Patients by Gender'
                    }
                }
            }
        });
    },

    initAppointmentStatusChart() {
        const ctx = document.getElementById('appointmentStatusChart');
        if (!ctx) return;

        this.charts.appointmentStatus = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: [],
                datasets: [{
                    data: [],
                    backgroundColor: ['#4BC0C0', '#FF6384', '#FF9F40', '#9966FF'],
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                ...this.defaultChartOptions,
                plugins: {
                    ...this.defaultChartOptions.plugins,
                    title: {
                        display: true,
                        text: 'Appointments by Status'
                    }
                }
            }
        });
    },
