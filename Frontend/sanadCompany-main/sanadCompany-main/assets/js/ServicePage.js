// جلب الخدمات لعرض القسم النصي (services-section)
async function loadServices() {
  try {
    const res = await fetch('http://localhost:5261/api/Services?lang=ar');
    const services = await res.json();
    const container = document.getElementById('services-section');
    container.innerHTML = ''; // تفريغ المحتوى القديم

    services.forEach(service => {
      // عنوان الخدمة
      const h5 = document.createElement('h5');
      h5.className = 'text-white mt-4';
      h5.textContent = service.title;
      container.appendChild(h5);

      // التفاصيل كنقاط قائمة
      if (service.details && service.details.length > 0) {
        const ul = document.createElement('ul');
        ul.className = 'text-white-50';
        service.details.forEach(detail => {
          const li = document.createElement('li');
          li.innerText = detail;
          ul.appendChild(li);
        });
        container.appendChild(ul);
      }
    });

    // الفقرة الختامية
    const finalText = document.createElement('p');
    finalText.className = 'text-white-50 mt-4';
    finalText.textContent = `في شركة سند، نحن لسنا مجرد مزودين لخدمات، بل شريك استراتيجي يقود نجاحك الرقمي ونموك المدعوم بالذكاء الاصطناعي. سواء كنت تبدأ مشروعك أو توسع نطاقه، نقدم حلولاً ذكية تنمو مع عملك.`;
    container.appendChild(finalText);

  } catch (error) {
    console.error('Failed to load services:', error);
  }
}

// جلب الخدمات لعرض بطاقات الخدمات (servicesContainer)
async function loadServices2() {
  try {
    const res = await fetch('http://localhost:5261/api/Services?lang=ar');
    const services = await res.json();
    const container = document.getElementById('servicesContainer');
    container.innerHTML = ''; // تفريغ المحتوى القديم

    services.forEach(service => {
      const col = document.createElement('div');
      col.className = 'col-md-6 col-lg-4';

      col.innerHTML = `
        <a href="services.html" class="text-decoration-none text-white">
          <div class="card h-100 border-0 shadow-sm p-3">
            <div class="card-body">
              <h5 class="card-title text-white">${service.title}</h5>
              <p class="card-text small text-white-50 mt-3">
                ${service.details && service.details.length > 0
                  ? service.details.map(d => `<span>${d}</span>`).join('<br>')
                  : 'لا توجد تفاصيل متاحة.'}
              </p>
            </div>
          </div>
        </a>
      `;
      container.appendChild(col);
    });

  } catch (error) {
    console.error('Failed to load services:', error);
  }
}

// عند اكتمال تحميل الـ DOM نحدد أي دالة ننادي بناءً على وجود العنصر
document.addEventListener('DOMContentLoaded', () => {
  if (document.getElementById('services-section')) {
    loadServices();
  }
  if (document.getElementById('servicesContainer')) {
    loadServices2();
  }
});
