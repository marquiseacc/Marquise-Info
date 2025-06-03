document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    fetchWithLoading('/CRM/Contract/ContractList', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    }, '#contract-list')
        .then(data => {
            if (data) {
                const rows = document.querySelectorAll("#contract-list table tbody tr");

                if (rows.length > 0) {
                    const firstRow = rows[0];
                    const firstContractId = firstRow.getAttribute('data-contract-id');
                    firstRow.classList.add('selected-row');
                    loadContractDetail(firstContractId);
                }

                rows.forEach(row => {
                    row.addEventListener('click', function () {
                        rows.forEach(r => r.classList.remove('selected-row'));
                        this.classList.add('selected-row');

                        const contractId = this.getAttribute('data-contract-id');
                        loadContractDetail(contractId);
                    });
                });
            }
        })
        .catch(error => {
            console.error('❌ خطا در دریافت لیست قراردادها:', error);
            const container = document.querySelector('#contract-list');

            container.innerHTML = `
            <div class="card h-100 shadow-sm border-danger">
                <div class="card-body text-center">
                    <img src="/Content/Images/error-page.png" alt="صفحه خطا" class="img-fluid mb-3" style="max-width: 200px;" />
                    <h6 class="mb-2">خطا در دریافت لیست قراردادها</h6>
                    <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات قراردادها پیش آمده است. لطفاً دوباره تلاش کنید.</p>
                </div>
            </div>
        `;
        });

    function loadContractDetail(contractId) {
        fetchWithLoading('/CRM/Contract/Detail?contractId=' + contractId, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }, '#DetailContract')
            .catch(error => {
                console.error('❌ خطا در دریافت جزئیات قرارداد:', error);
                const container = document.querySelector('#DetailContract');

                container.innerHTML = `
            <div class="card h-100 shadow-sm border-danger">
                <div class="card-body text-center">
                    <img src="/Content/Images/error-page.png" alt="صفحه خطا" class="img-fluid mb-3" style="max-width: 200px;" />
                    <h6 class="mb-2">خطا در دریافت جزئیات قرارداد</h6>
                    <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات این قرارداد پیش آمده است. لطفاً دوباره تلاش کنید.</p>
                </div>
            </div>
        `;
            });
    }
});
