var username = ""

function initialize() {
	redirectLoginPage();
	getUsername();
	
}
function getUsername(){
	username = localStorage.getItem('username');
	console.log("Logged in users username: " + username)

}

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

    try {
        const data = await getData("Course");
        console.log(data);

        var newcontent = 
			`<div>
                    <h3>Kurzusok:</h3>
            `;
        data.forEach(course => {
            newcontent += `
                <div>
                    <h4>${course.Name}</h4>
                    <p><strong>Neptun kód:</strong> ${course.Code}</p>
                    <p><strong>Tanszék:</strong> ${course.Department}</p>
                    <p><strong>Kredit:</strong> ${course.Credit}</p>
                </div>
            `;
        });
		newcontent += 
			`</div>`;
		textArea.innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

async function showEvents() {
	var textArea = document.getElementById("mainContent");
	var newcontent = `
        <h2>Következő esemény</h2>
    `;
	try {
        const eventdata = await getData(`User/nextevent/${username}`);
        console.log(eventdata);

        newcontent += `
                <div>
                    <h4>${eventdata.Name}</h4>
                    <p><strong>Kurzus(egyenlőre kód, nemtom hogy kéne nevet):</strong> ${eventdata.Course_Id}</p>
                    <p><strong>Leírás:</strong> ${eventdata.Description}</p>
                    <p><strong>Kredit:</strong> ${eventdata.Date}</p>
                </div>
            `;
		textArea.innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

async function showCoursesByDept(){
    var textArea = document.getElementById("mainContent");
    var dept = document.getElementById("deptInput").value;
    try{
        const data = await getData(`Course/dept/${dept}`);
        console.log(data);
        var newcontent = 
			`<div>
                    <h3>Kurzusok a(z) ${dept} tankszéken:</h3>
            `;
        data.forEach(course => {
            newcontent += `
                <div>
                    <h4>${course.Name}</h4>
                    <p><strong>Neptun kód:</strong> ${course.Code}</p>
                    <p><strong>Tanszék:</strong> ${course.Department}</p>
                    <p><strong>Kredit:</strong> ${course.Credit}</p>
                </div>
            `;
        });
		newcontent += 
			`</div>`;
		textArea.innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

function redirectLoginPage() {
	document.getElementById("logoutButton").addEventListener("click", function() {window.location.href = "index.html";});
		
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
