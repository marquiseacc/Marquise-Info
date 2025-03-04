var secretKey = "Marquise33";  // توصیه می‌شود از یک کلید امن استفاده کنید


document.addEventListener("DOMContentLoaded", function () {
    console.log("1111")
    // خواندن کوکی‌ها
    var encryptedUsername = getCookie("username"); // کوکی نام کاربری
    var encryptedPassword = getCookie("password"); // کوکی رمز عبور
    var encryptedRememberMe = getCookie("rememberMe"); // کوکی rememberMe

    // رمزگشایی کوکی‌ها
    var username = decryptData(encryptedUsername);
    var password = decryptData(encryptedPassword);
    var rememberMe = decryptData(encryptedRememberMe);
    console.log(rememberMe);
    // پر کردن فیلدهای ورودی
    if (username) {
        document.getElementById("username").value = username; // مقداردهی فیلد یوزرنیم
    }
    if (password) {
        document.getElementById("password").value = password; // مقداردهی فیلد پسورد
    }

    // مقداردهی چک‌باکس rememberMe
    if (rememberMe === "true") {
        document.getElementById("rememberMe").checked = true;
    } else {
        document.getElementById("rememberMe").checked = false;
    }
});

function getCookie(name) {
    var nameEq = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(nameEq) == 0) {
            return c.substring(nameEq.length, c.length);
        }
    }
    return "";
}

// تابع برای رمزگشایی داده‌ها
function decryptData(encryptedData) {
    try {
        var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Base64);  // کلید هش‌شده برای رمزگشایی

        // بررسی اینکه آیا داده رمزنگاری‌شده به درستی وجود دارد یا نه
        if (!encryptedData) {
            console.error("No encrypted data found.");
            return null;
        }

        // رمزگشایی داده‌ها
        var bytes = CryptoJS.AES.decrypt(encryptedData, hashedKey);

        // اگر رمزگشایی موفقیت‌آمیز بود، نتیجه را به رشته تبدیل می‌کنیم
        var decryptedData = bytes.toString(CryptoJS.enc.Utf8);

        // اگر داده رمزگشایی شده خالی است، ممکن است مشکلی در رمزگشایی وجود داشته باشد
        if (!decryptedData) {
            console.error("Failed to decrypt the data.");
            return null;
        }

        return decryptedData;  // برگرداندن داده‌های رمزگشایی‌شده
    } catch (e) {
        console.error("Error decrypting data: ", e);
        return null;  // در صورت بروز خطا، مقدار null بازگشت داده می‌شود
    }
}
