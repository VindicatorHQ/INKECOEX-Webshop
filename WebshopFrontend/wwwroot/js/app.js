window.setBodyTheme = (theme) => {
    const body = document.body;

    body.classList.remove('theme-light', 'theme-dark');

    if (theme === 'dark') {
        body.classList.add('theme-dark');
    } else {
        body.classList.add('theme-light');
    }
};

function drawCanvasChart(canvasId, type, labels, values, title, color) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    const width = canvas.width = canvas.offsetWidth;
    const height = canvas.height = canvas.offsetHeight;

    ctx.clearRect(0, 0, width, height);

    if (values.length === 0) return;

    const maxValue = Math.max(...values);
    const padding = 20;
    const chartHeight = height - 2 * padding;
    const chartWidth = width - 2 * padding;
    const barWidth = chartWidth / values.length;

    ctx.font = '10px sans-serif';
    ctx.fillStyle = '#666';

    ctx.fillText(title, padding, padding / 2);

    ctx.beginPath();
    ctx.strokeStyle = '#ccc';
    ctx.moveTo(padding, height - padding);
    ctx.lineTo(width - padding, height - padding);
    ctx.moveTo(padding, padding);
    ctx.lineTo(padding, height - padding);
    ctx.stroke();

    ctx.fillStyle = color;
    ctx.strokeStyle = color;

    for (let i = 0; i < values.length; i++) 
    {
        const value = values[i];
        const label = labels[i];
        const barHeight = (value / maxValue) * chartHeight;
        const x = padding + i * barWidth;
        const y = height - padding - barHeight;

        if (type === 'bar') 
        {
            ctx.fillRect(x + barWidth * 0.1, y, barWidth * 0.8, barHeight);
        } 
        else if (type === 'line' && i > 0) 
        {
            const prevValue = values[i - 1];
            const prevBarHeight = (prevValue / maxValue) * chartHeight;
            const prevX = padding + (i - 1) * barWidth + barWidth / 2;
            const prevY = height - padding - prevBarHeight;

            ctx.beginPath();
            ctx.moveTo(prevX, prevY);
            ctx.lineTo(x + barWidth / 2, y);
            ctx.stroke();

            ctx.beginPath();
            ctx.arc(x + barWidth / 2, y, 3, 0, Math.PI * 2);
            ctx.fill();
        }

        if (i % 5 === 0 || values.length <= 5) {
            ctx.save();
            ctx.translate(x + barWidth * 0.5, height - padding + 5);
            ctx.rotate(Math.PI / 4);
            ctx.fillStyle = '#666';
            ctx.fillText(label.substring(5), 0, 0);
            ctx.restore();
        }
    }

    ctx.fillStyle = '#666';
    ctx.textAlign = 'right';
    ctx.fillText(maxValue.toFixed(2), padding - 5, padding);
    ctx.textAlign = 'left';
}
