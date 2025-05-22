document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        const firstRow = rows[0];
        const firstContractId = firstRow.getAttribute('data-contract-id');
        firstRow.classList.add('selected-row'); // انتخاب اولین ردیف
        loadContractDetail(firstContractId);
    }

    rows.forEach(row => {
        row.addEventListener('click', function () {
            // حذف انتخاب قبلی از همه ردیف‌ها
            rows.forEach(r => r.classList.remove('selected-row'));

            // افزودن کلاس به ردیف کلیک‌شده
            this.classList.add('selected-row');

            const contractId = this.getAttribute('data-contract-id');
            loadContractDetail(contractId);
        });
    });

    function loadContractDetail(contractId) {
        fetch('/CRM/Contract/Detail?contractId=' + contractId, {
            method: 'GET'
        })
            .then(response => response.text())
            .then(data => {
                document.querySelector('#DetailContract').innerHTML = data;
            })
            .catch(error => {
                console.error('خطا در بارگذاری جزئیات:', error);
            });
    }
});
