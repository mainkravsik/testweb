// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Вспомогательные функции для работы с API тестером

// Форматирование JSON в текстовом поле
function formatJsonTextarea(textareaId) {
    const textarea = document.getElementById(textareaId);
    if (!textarea) return;
    
    try {
        let json = JSON.parse(textarea.value);
        textarea.value = JSON.stringify(json, null, 2);
    } catch (e) {
        // Если не валидный JSON, оставляем как есть
        console.log('Не удалось распарсить JSON');
    }
}

// Форматирование всех JSON текстовых полей на странице
function setupJsonFormatting() {
    const jsonTextareas = document.querySelectorAll('.json-textarea');
    
    jsonTextareas.forEach(textarea => {
        textarea.addEventListener('blur', function() {
            try {
                let json = JSON.parse(this.value);
                this.value = JSON.stringify(json, null, 2);
            } catch (e) {
                // Если не валидный JSON, оставляем как есть
            }
        });
        
        // Попытка форматирования при загрузке страницы
        try {
            if (textarea.value) {
                let json = JSON.parse(textarea.value);
                textarea.value = JSON.stringify(json, null, 2);
            }
        } catch (e) {
            // Если не валидный JSON, оставляем как есть
        }
    });
}

// Копирование содержимого в буфер обмена
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function() {
        alert('Скопировано в буфер обмена!');
    }, function(err) {
        console.error('Не удалось скопировать в буфер: ', err);
    });
}

// Инициализация всех компонентов при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    setupJsonFormatting();
});
