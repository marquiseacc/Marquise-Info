class ArticleEditor {
    constructor() {
        this.sectionCount = 1; // شمارشگر بخش‌ها
    }

    // افزودن بخش عنوان
    addTitle() {
        const id = `section-${this.sectionCount}`;
        const newSection = `
            <div class="article-section d-flex justify-content-between align-items-center" id="${id}">
                <label for="title-${this.sectionCount}">عنوان:</label>
                <input type="text" id="title-${this.sectionCount}" name="title[]" placeholder="عنوان خود را وارد کنید..." oninput="articleEditor.updateTitle(${this.sectionCount})" required />
                <button class="blog-delete-button" onclick="articleEditor.removeSection('${id}')"><i class="bi bi-trash3-fill"></i></button>
            </div>
        `;
        document.getElementById("articleSections").insertAdjacentHTML('beforeend', newSection);
        this.addToPreview(`عنوان ${this.sectionCount}`, id, 'blog-title-class');
        this.sectionCount++;
    }

    // افزودن بخش تصویر
    addImage() {
        const id = `section-${this.sectionCount}`;
        const newSection = `
            <div class="article-section d-flex justify-content-between align-items-center" id="${id}">
                <label for="image-${this.sectionCount}">تصویر:</label>
                <input type="file" id="image-${this.sectionCount}" name="image[]" accept="image/*" onchange="articleEditor.updateImage(this, ${this.sectionCount})" required />
                <button class="blog-delete-button" onclick="articleEditor.removeSection('${id}')"><i class="bi bi-trash3-fill"></i></button>
            </div>
        `;
        document.getElementById("articleSections").insertAdjacentHTML('beforeend', newSection);
        this.addToPreview(`تصویر ${this.sectionCount}`, id, 'blog-image-class');
        this.sectionCount++;
    }

    // افزودن بخش توضیحات
    addDescription() {
        const id = `section-${this.sectionCount}`;
        const newSection = `
            <div class="article-section d-flex justify-content-between align-items-center" id="${id}">
                <label for="description-${this.sectionCount}">توضیحات:</label>
                <textarea id="description-${this.sectionCount}" name="description[]" rows="3" placeholder="توضیحات خود را وارد کنید..." oninput="articleEditor.updateDescription(${this.sectionCount})" required></textarea>
                <button class="blog-delete-button" onclick="articleEditor.removeSection('${id}')"><i class="bi bi-trash3-fill"></i></button>
            </div>
        `;
        document.getElementById("articleSections").insertAdjacentHTML('beforeend', newSection);
        this.addToPreview(`توضیحات ${this.sectionCount}`, id, 'blog-description-class');
        this.sectionCount++;
    }

    // به‌روزرسانی عنوان در پیش‌نمایش
    updateTitle(index) {
        const titleValue = document.getElementById(`title-${index}`).value;
        this.updatePreviewContent(index, titleValue, 'blog-title-class');
    }

    // به‌روزرسانی توضیحات در پیش‌نمایش
    updateDescription(index) {
        const descriptionValue = document.getElementById(`description-${index}`).value;
        this.updatePreviewContent(index, descriptionValue.replace(/\n/g, '<br>'), 'blog-description-class');
    }

    // پیش‌نمایش تصویر
    updateImage(input, index) {
        const file = input.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = (e) => {
            const previewElement = document.getElementById(`preview-section-${index}`);
            if (previewElement) {
                previewElement.innerHTML = `<img src="${e.target.result}" class="blog-image-class" />`;
            }
        };
        reader.readAsDataURL(file);
    }

    // افزودن به پیش‌نمایش
    addToPreview(content, id, className) {
        const previewList = document.getElementById('previewContent');
        const item = document.createElement('div');
        item.id = `preview-section-${id.split('-')[1]}`;
        item.textContent = content;
        item.className = className;
        previewList.appendChild(item);
    }

    // به‌روزرسانی محتوا در پیش‌نمایش
    updatePreviewContent(index, content, className) {
        let previewElement = document.getElementById(`preview-section-${index}`);
        if (!previewElement) {
            previewElement = document.createElement('div');
            previewElement.id = `preview-section-${index}`;
            document.getElementById('previewContent').appendChild(previewElement);
        }
        previewElement.className = `preview-item ${className}`;
        previewElement.innerHTML = content;
    }

    // حذف یک بخش
    removeSection(id) {
        document.getElementById(id)?.remove();
        document.getElementById(`preview-section-${id.split('-')[1]}`)?.remove();
    }
}

// ایجاد شیء برای مدیریت ویرایشگر
const articleEditor = new ArticleEditor();

// مدیریت ارسال فرم
function handleAddBlogFormSubmit(event) {
    event.preventDefault();

    const articleTitle = document.getElementById("articleTitle").value;
    const articleCategory = document.getElementById("articleCategory").value;
    const articleAuthor = document.getElementById("articleAuthor").value;

    const previewSections = document.querySelectorAll("#previewContent .preview-item");
    let previewHTML = "";

    previewSections.forEach(section => {
        previewHTML += section.outerHTML;
    });

    const data = {
        Title: articleTitle,
        Category: articleCategory,
        Author: articleAuthor,
        ArticleContent: previewHTML
    };

    fetch('/api/ManageBlog/AddBlog', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                alert("مقاله با موفقیت ثبت شد.");
                window.location.reload();
            } else {
                alert("خطایی در ثبت مقاله رخ داد.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("خطا در ارتباط با سرور.");
        });
}
