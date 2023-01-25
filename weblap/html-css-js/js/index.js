// javascript for index.html

const renderJaratok = async () => {
    let uri = 'http://localhost:5000/jaratok';

    const res = await  fetch(uri)
    const jaratok = await res.json();

    let template = '';
    jaratok.forEach(jaratok => {
        template +=
        <div class="jaratok"></div>

        
    })
}

window.addEventListener('DOMContentLoaded', () => renderJaratok());