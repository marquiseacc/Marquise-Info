window.fetchWithLoading = function (url, options = {}, targetSelector) {
    const target = document.querySelector(targetSelector);
    const globalLoader = document.getElementById("global-loading");

    if (globalLoader && globalLoader.style.display !== "none") {
        globalLoader.style.display = "none";
    }

    if (target) {
        if (getComputedStyle(target).position === 'static') {
            target.style.position = 'relative';
        }

        const old = target.querySelector('.spinner-inline-wrapper');
        if (old) old.remove();

        target.classList.add('loader-placeholder');

        const loader = document.createElement('div');
        loader.className = 'spinner-inline-wrapper';
        loader.innerHTML = `
            <div class="spinner">
                <div class="dot1"></div>
                <div class="dot2"></div>
            </div>`;
        target.appendChild(loader);
    }

    const token = localStorage.getItem('jwtToken');
    const headers = {
        ...options.headers,
        ...(token ? { 'Authorization': 'Bearer ' + token } : {})
    };

    return fetch(url, {
        ...options,
        headers: headers,
        __internal: true
    })
        .then(response => {
            if (response.status === 401) {
                localStorage.removeItem('jwtToken');
                window.location.href = '/Auth/SendOtp';
                return Promise.reject(new Error('Unauthorized'));
            }

            if (!response.ok) {
                return Promise.reject(new Error(`خطا: ${response.status}`));
            }

            return response.text();
        })
        .then(data => {
            if (target && data) {
                target.innerHTML = data;
            }
            return data;
        })
        .finally(() => {
            if (target) {
                const inlineLoader = target.querySelector('.spinner-inline-wrapper');
                if (inlineLoader) inlineLoader.remove();

                target.classList.remove('loader-placeholder');
            }
        });
};

// داخل DOMContentLoaded
document.addEventListener('DOMContentLoaded', function () {
    const globalLoader = document.getElementById("global-loading");
    let activeRequests = 0;
    const originalFetch = window.fetch;

    window.fetch = async function (url, options = {}) {
        const isInternalRequest = options.__internal === true;

        // فقط اگه درخواست داخلی نبود، لودینگ سراسری رو نمایش بده
        if (!isInternalRequest && globalLoader) {
            activeRequests++;
            globalLoader.style.display = "flex";
        }

        try {
            const response = await originalFetch(url, options);
            if (!response.ok) throw new Error(`خطا در دریافت: ${response.status}`);
            return response;
        } catch (error) {
            throw error;
        } finally {
            if (!isInternalRequest && globalLoader) {
                activeRequests--;
                if (activeRequests === 0) {
                    globalLoader.style.display = "none";
                }
            }
        }
    };
});
