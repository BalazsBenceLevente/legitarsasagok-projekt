// javascript for details.html
const id = new URLSearchParams(window.location.search).get('id');

const renderDetails = async () => {
    const res = fetch('http://localhost:5000/jaratok/' + id);
    const jaratok = await res.json();

    const template = `
      <h1>${jaratok.legitarsasag}</h1>
    
    `
    
}

window.addEventListener('DOMContentLoaded', () => renderJaratok());