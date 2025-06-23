document.addEventListener('DOMContentLoaded', () => {
  const lang = window.location.pathname.includes('/ar') ? 'ar' : 'en';

  loadServices(lang);
  loadServices2(lang);
});

async function loadServices(lang) {
  try {
    const res = await fetch(`http://localhost:5261/api/Services?lang=${lang}`);
    const services = await res.json();
    const container = document.getElementById('services-section');
    container.innerHTML = '';

    services.forEach(service => {
      const h5 = document.createElement('h5');
      h5.className = 'text-white mt-4';
      h5.textContent = service.title;
      container.appendChild(h5);

      if (Array.isArray(service.details) && service.details.length > 0) {
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

    const finalText = document.createElement('p');
    finalText.className = 'text-white-50 mt-4';
    finalText.innerHTML = lang === 'ar'
      ? `في شركة سند، لسنا مجرد مزود خدمة — بل شريك استراتيجي نشط في قيادة نجاحك الرقمي ونموك المدعوم بالذكاء الاصطناعي.`
      : `At Sanad Company, we are not just a service provider — we are a strategic partner in your digital growth.`;

    container.appendChild(finalText);

  } catch (error) {
    console.error('Error loading services section:', error);
  }
}

async function loadServices2(lang) {
  try {
    const res = await fetch(`http://localhost:5261/api/Services?lang=${lang}`);
    const services = await res.json();
    const container = document.getElementById('servicesContainer');
    container.innerHTML = '';

    services.forEach(service => {
      const col = document.createElement('div');
      col.className = 'col-md-6 col-lg-4';

      const link = document.createElement('a');
      link.href = '#';
      link.className = 'text-decoration-none text-white';

      const card = document.createElement('div');
      card.className = 'card h-100 border-0 shadow-sm p-3';

      const cardBody = document.createElement('div');
      cardBody.className = 'card-body';

      const title = document.createElement('h5');
      title.className = 'card-title text-white';
      title.textContent = service.title;

      const description = document.createElement('p');
      description.className = 'card-text small text-white-50 mt-3';

      if (Array.isArray(service.details) && service.details.length > 0) {
        description.innerHTML = service.details.map(d => `<span>${d}</span>`).join('<br>');
      } else {
        description.innerText = lang === 'ar' ? 'لا توجد تفاصيل.' : 'No details available.';
      }

      cardBody.appendChild(title);
      cardBody.appendChild(description);
      card.appendChild(cardBody);
      link.appendChild(card);
      col.appendChild(link);
      container.appendChild(col);
    });

  } catch (error) {
    console.error('Error loading services grid:', error);
  }
}
