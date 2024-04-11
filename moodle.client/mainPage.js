function showHome() {
    document.getElementById("mainContent").innerHTML = `
        <h2>Üdvözlünk a Moodle rendszerben</h2>
        <p>Kezdőlap.</p>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla sagittis ultricies dolor quis ultrices. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae;
    Maecenas ornare suscipit orci ac cursus. Duis facilisis nulla vehicula nisi lobortis, vel fermentum eros blandit. Mauris mattis ante quis finibus venenatis. 
    Phasellus bibendum quis mi eget pretium. Nam sit amet massa lectus. Vivamus varius interdum urna non fermentum. 
    Morbi tincidunt, metus sit amet fermentum vulputate, ex arcu luctus ante, vitae semper urna purus id nunc. Morbi sit amet lectus eu diam malesuada suscipit.</p>
    `;
}

async function showCourses() {
    var textArea = document.getElementById("mainContent");

    try{        
        const data = await getData("Course");
        console.log(data);
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}

/*function showMyCourses() {
    var newelement = `
        <h2>Saját Kurzusok</h2>
        <ul>
    `;
    for (let i = 1; i < 16; i++) {
        newelement += "<li> Saját kurzus " + (i) + "</li>";
    }
    newelement += "</ul>";

    document.getElementById("mainContent").innerHTML = newelement;
}

function redirectLoginPage() {
    document.getElementById("redirectButton").addEventListener("click", function() {window.location.href = "index.html";});

}

function showEvents() {
    var newelement = `
        <h2>Közelgő események</h2>
    `;
    for (let i = 1; i < 4; i++) {
        newelement += '<div class="event"> <h3>Esemény ' + (i) + '</h3><p>Esemény leírása ' + (i) + '</p></div>';
    }

    document.getElementById("eventsContent").innerHTML = newelement;
}*/
