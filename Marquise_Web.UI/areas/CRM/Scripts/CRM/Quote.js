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
        });

    function loadQuoteDetail(quoteId) {
        fetchWithLoading('/CRM/Quote/Detail?quoteId=' + encodeURIComponent(quoteId), {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }, '#DetailQuote');
    }
});
