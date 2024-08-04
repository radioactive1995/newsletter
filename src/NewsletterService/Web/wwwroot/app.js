window.toggleLoading = (show) => {
    const loadingDiv = document.getElementById('loading');
    if (loadingDiv) {
        if (show) {
            loadingDiv.classList.add('loader');
        } else {
            loadingDiv.classList.remove('loader');
        }
    }
};
