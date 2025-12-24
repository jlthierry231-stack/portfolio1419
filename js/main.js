document.addEventListener('click', e=>{
  if(e.target.matches('a[href^="#"]')){
    const id = e.target.getAttribute('href')
    if(id.length>1){
      e.preventDefault();
      document.querySelector(id).scrollIntoView({behavior:'smooth',block:'start'});
    }
  }
});

// small entrance animation for cards
window.addEventListener('load', ()=>{
  document.querySelectorAll('.card').forEach((c,i)=>{
    c.style.opacity=0; c.style.transform='translateY(18px)';
    setTimeout(()=>{c.style.transition='all .45s cubic-bezier(.2,.9,.2,1)'; c.style.opacity=1; c.style.transform='translateY(0)';}, 120*i)
  })
})

// mobile menu toggle
document.addEventListener('click', e=>{
  if(e.target.classList && e.target.classList.contains('menu-toggle')){
    const nav = document.querySelector('header nav');
    if(nav.style.display==='flex'){nav.style.display='none'; e.target.textContent='☰'} else {nav.style.display='flex'; e.target.textContent='✕'}
  }
})

// animate logo on scroll
window.addEventListener('scroll', () => {
  const logo = document.querySelector('.logo');
  if (window.scrollY > 50) {
    logo.classList.add('animated');
  } else {
    logo.classList.remove('animated');
  }
});
