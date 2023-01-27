// javascript for index.html

const renderJaratok = async () => {
    let url = 'http://localhost:5000/jaratok';

    const res = await  fetch(url)
    const jaratok = await res.json();
    
    let template = '';
    jaratok.forEach(jaratok => {
        template += `
          <div class="jaratok">
            <h2>${jaratok.legitarsasag}</h2>
            <p><small>${jaratok.ido} Repülési idő percben</small></p>
            <a href="/details.html">mutass többet...</a>
          </div>
        `
        
    })
    container.innerHTML = template;

}

window.addEventListener('DOMContentLoaded', () => renderJaratok());