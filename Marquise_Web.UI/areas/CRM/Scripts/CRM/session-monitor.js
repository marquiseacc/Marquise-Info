(function () {
    // تنظیم مدت‌زمان مجاز برای inactivity (بر حسب دقیقه)
    const MAX_INACTIVE_MINUTES = 60;

    let lastActivity = new Date().getTime();

    // رویدادهایی که نشان‌دهنده فعالیت کاربر هستند
    document.addEventListener('mousemove', resetTimer);
    document.addEventListener('keydown', resetTimer);
    document.addEventListener('click', resetTimer);
    document.addEventListener('scroll', resetTimer);

    function resetTimer() {
        lastActivity = new Date().getTime();
    }

    // بررسی inactivity هر دقیقه
    setInterval(() => {
        const now = new Date().getTime();
        const diff = now - lastActivity;
        const minutesInactive = diff / (1000 * 60);

        if (minutesInactive > MAX_INACTIVE_MINUTES) {
            // حذف توکن از localStorage (در صورت استفاده)
            localStorage.removeItem('jwtToken');

            // هدایت کاربر به صفحه ورود
            window.location.href = '/Auth/SendOtp';
        }
    }, 60000); // بررسی هر 60 ثانیه
})();


