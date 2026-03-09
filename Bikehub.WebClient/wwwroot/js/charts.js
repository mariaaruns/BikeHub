// BikeHub Chart.js initialization — theme-aware

function bh_tc() {
    const w = document.querySelector('.bh-wrapper');
    if (!w) return { grid: '#2A3350', text: '#7888AA', elevated: '#1E2535', border: '#2A3350' };
    const s = getComputedStyle(w);
    return {
        grid:     s.getPropertyValue('--border').trim(),
        text:     s.getPropertyValue('--tx-s').trim(),
        elevated: s.getPropertyValue('--bg-e').trim(),
        border:   s.getPropertyValue('--border').trim(),
    };
}

function bh_destroyAll() {
    Object.values(Chart.instances).forEach(c => c?.destroy());
}

function bh_grad(ctx, color, h = 250) {
    const g = ctx.createLinearGradient(0, 0, 0, h);
    g.addColorStop(0, color + '55');
    g.addColorStop(1, color + '00');
    return g;
}

window.initDashboardCharts = function (data) {
    bh_destroyAll();
    const tc = bh_tc();
    Chart.defaults.color = tc.text;
    Chart.defaults.borderColor = tc.grid;
    Chart.defaults.font.family = "'Instrument Sans', sans-serif";

    const C = { orange: '#FF6B35', teal: '#4ECDC4', blue: '#6C8EF5', yellow: '#F7B731', green: '#44CF6C', red: '#FC5C7D' };

    // Revenue line
    const rev = document.getElementById('revenueChart');
    if (rev) {
        const ctx = rev.getContext('2d');
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: data.revenue.labels,
                datasets: [{
                    label: 'Revenue',
                    data: data.revenue.values,
                    borderColor: C.orange,
                    backgroundColor: bh_grad(ctx, C.orange),
                    borderWidth: 2.5, tension: 0.45, pointRadius: 5,
                    pointBackgroundColor: C.orange, pointBorderColor: tc.elevated, pointBorderWidth: 2,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                interaction: { intersect: false, mode: 'index' },
                plugins: {
                    legend: { display: false },
                    tooltip: { backgroundColor: tc.elevated, borderColor: tc.border, borderWidth: 1, callbacks: { label: ctx => ' $' + Math.round(ctx.raw).toLocaleString() } }
                },
                scales: {
                    x: { grid: { color: tc.grid + '44' }, ticks: { color: tc.text } },
                    y: { grid: { color: tc.grid + '44' }, ticks: { color: tc.text, callback: v => '$' + (v / 1000).toFixed(0) + 'k' } }
                }
            }
        });
    }

    // Status donut
    const donut = document.getElementById('statusDonut');
    if (donut) {
        new Chart(donut.getContext('2d'), {
            type: 'doughnut',
            data: {
                labels: ['Pending', 'Completed', 'In Service', 'Other'],
                datasets: [{ data: data.status.values, backgroundColor: [C.yellow, C.green, C.orange, C.blue], borderWidth: 0, hoverOffset: 8 }]
            },
            options: { cutout: '74%', plugins: { legend: { display: false } } }
        });
    }

    // Category bar
    const catEl = document.getElementById('catBar');
    if (catEl) {
        new Chart(catEl.getContext('2d'), {
            type: 'bar',
            data: {
                labels: data.cats.labels,
                datasets: [{ label: 'Products', data: data.cats.values, backgroundColor: C.blue + 'CC', borderRadius: 6, borderSkipped: false }]
            },
            options: {
                responsive: true, indexAxis: 'y',
                plugins: { legend: { display: false } },
                scales: { x: { grid: { color: tc.grid + '44' }, ticks: { color: tc.text } }, y: { grid: { display: false }, ticks: { color: tc.text, font: { size: 11 } } } }
            }
        });
    }

    // Stock bar
    const stockEl = document.getElementById('stockBar');
    if (stockEl) {
        const stockColors = data.stock.values.map(v => v < 5 ? C.red + 'CC' : v < 10 ? C.yellow + 'CC' : C.green + 'CC');
        new Chart(stockEl.getContext('2d'), {
            type: 'bar',
            data: {
                labels: data.stock.labels,
                datasets: [{ label: 'Stock', data: data.stock.values, backgroundColor: stockColors, borderRadius: 6, borderSkipped: false }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: false }, tooltip: { callbacks: { label: ctx => ' ' + ctx.raw + ' units' } } },
                scales: { x: { grid: { display: false }, ticks: { color: tc.text, font: { size: 10 } } }, y: { grid: { color: tc.grid + '44' }, ticks: { color: tc.text } } }
            }
        });
    }
};
