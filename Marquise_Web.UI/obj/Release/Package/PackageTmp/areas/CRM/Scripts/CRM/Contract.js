document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    fetch('/CRM/Contract/ContractList', {
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
                document.getElementById('contract-list').innerHTML = data;

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
            console.error('❌ خطا در ارسال درخواست:', error);
        });

    function loadContractDetail(contractId) {
        fetch('/CRM/Contract/Detail?contractId=' + contractId, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("خطا در دریافت جزئیات قرارداد");
                }
                return response.text();
            })
            .then(data => {
                document.querySelector('#DetailContract').innerHTML = data;
            })
            .catch(error => {
                console.error('❌ خطا در بارگذاری جزئیات قرارداد:', error);
            });
    }
});
