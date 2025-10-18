window.setBodyTheme = (theme) => {
    const body = document.body;

    body.classList.remove('theme-light', 'theme-dark');

    if (theme === 'dark') {
        body.classList.add('theme-dark');
    } else {
        body.classList.add('theme-light');
    }
};