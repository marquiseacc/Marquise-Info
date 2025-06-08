function convertToPersianNumbers(element) {
    const persianNumbers = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    // Function to replace English numbers with Persian numbers
    function replaceWithPersianNumbers(text) {
        return text.replace(/\d/g, function (digit) {
            return persianNumbers[digit];
        });
    }

    // Traverse through all child nodes
    element.childNodes.forEach(function (node) {
        if (node.nodeType === Node.TEXT_NODE) {
            // Only change text nodes
            node.textContent = replaceWithPersianNumbers(node.textContent);
        } else if (node.nodeType === Node.ELEMENT_NODE) {
            // Recursively traverse through element nodes
            convertToPersianNumbers(node);
        }
    });
}
document.addEventListener("DOMContentLoaded", function () {
    convertToPersianNumbers(document.body);
});
