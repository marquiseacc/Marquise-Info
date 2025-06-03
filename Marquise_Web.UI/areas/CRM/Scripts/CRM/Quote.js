document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("لطفاً وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    fetchWithLoading('/CRM/Quote/QuoteList', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    }, '#quote-list')
        .then(data => {
            if (!data) return;

            const rows = document.querySelectorAll("#quote-list table tbody tr");

            if (rows.length > 0) {
                const firstRow = rows[0];
                firstRow.classList.add('selected-row');
                const firstQuoteId = firstRow.getAttribute('data-quote-id');
                loadQuoteDetail(firstQuoteId);
            }

            rows.forEach(row => {
                row.addEventListener('click', function () {
                    if (this.classList.contains('selected-row')) return;

                    rows.forEach(r => r.classList.remove('selected-row'));
                    this.classList.add('selected-row');

                    const quoteId = this.getAttribute('data-quote-id');
                    loadQuoteDetail(quoteId);
                });
            });
        })
        .catch(error => {
            console.error('❌ خطا در دریافت لیست کووت‌ها:', error);
            const container = document.getElementById('quote-list');
            if (container) {
                container.innerHTML = `
                    <div class="card h-100 shadow-sm border-danger">
                        <div class="card-body text-center">
                            <img src="/Content/Images/error-page.png" alt="خطای لیست کووت‌ها" class="img-fluid mb-3" style="max-width: 200px;" />
                            <h6 class="mb-2">خطا در دریافت لیست پیشنهادات قیمت</h6>
                            <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات کووت‌ها پیش آمده است. لطفاً دوباره تلاش کنید.</p>
                        </div>
                    </div>
                `;
            }
        });

    function loadQuoteDetail(quoteId) {
        fetchWithLoading('/CRM/Quote/Detail?quoteId=' + encodeURIComponent(quoteId), {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }, '#DetailQuote')
            .catch(error => {
                console.error('❌ خطا در دریافت جزئیات کووت:', error);
                const detailContainer = document.getElementById('DetailQuote');
                if (detailContainer) {
                    detailContainer.innerHTML = `
                    <div class="card h-100 shadow-sm border-danger">
                        <div class="card-body text-center">
                            <img src="/Content/Images/error-page.png" alt="خطای جزئیات کووت" class="img-fluid mb-3" style="max-width: 200px;" />
                            <h6 class="mb-2">خطا در دریافت جزئیات پیشنهاد قیمت</h6>
                            <p class="font-13">لطفاً بعداً دوباره تلاش کنید.</p>
                        </div>
                    </div>
                `;
                }
            });
    }
});
