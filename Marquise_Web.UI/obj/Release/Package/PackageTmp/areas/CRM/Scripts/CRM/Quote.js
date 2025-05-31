document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    fetch('/CRM/Quote/QuoteList', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    })
        .then(response => {
            if (response.status === 401 || response.status === 403) {
                console.error(`❌ خطا در احراز هویت (${response.status}): دسترسی غیرمجاز یا توکن نامعتبر است.`);
                return '';
            }

            if (!response.ok) {
                console.error(`❌ خطای ناشناخته (${response.status}) هنگام دریافت اطلاعات.`);
                return '';
            }

            return response.text();
        })
        .then(data => {
            if (data) {
                document.getElementById('quote-list').innerHTML = data;

                // انتخاب اولین ردیف و بارگذاری جزئیات
                const rows = document.querySelectorAll("#quote-list table tbody tr");

                if (rows.length > 0) {
                    const firstRow = rows[0];
                    const firstQuoteId = firstRow.getAttribute('data-quote-id');
                    firstRow.classList.add('selected-row');
                    loadQuoteDetail(firstQuoteId);
                }

                // افزودن رویداد کلیک به ردیف‌ها
                rows.forEach(row => {
                    row.addEventListener('click', function () {
                        rows.forEach(r => r.classList.remove('selected-row'));
                        this.classList.add('selected-row');

                        const quoteId = this.getAttribute('data-quote-id');
                        loadQuoteDetail(quoteId);
                    });
                });
            }
        })
        .catch(error => {
            console.error('❌ خطا در ارسال درخواست:', error);
        });

    function loadQuoteDetail(quoteId) {
        fetch('/CRM/Quote/Detail?quoteId=' + quoteId, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
            .then(response => response.text())
            .then(data => {
                document.querySelector('#DetailQuote').innerHTML = data;
            })
            .catch(error => {
                console.error('❌ خطا در بارگذاری جزئیات:', error);
            });
    }
});

