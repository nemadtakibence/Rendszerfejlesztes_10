var username = localStorage.getItem('username');

function initialize() {
	redirectLoginPage();
	getUsername();
	
}

function getUsername(){
	username = localStorage.getItem('username');
	console.log("Logged in users username: " + username)
    //return username;
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

/*async function showCourses() {
    //var textArea = document.getElementById("mainContent");

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
                    <div>
                        <p><input type="button" value="Hallgatók listázása" onclick="showStudents()"></p>
                    </div>
                </div>
            `;
        });
		newcontent += 
			`</div>`;
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

async function showStudents(){
	var newcontent = `
        <h2>Hallgatók:</h2>
        <div>
    `;
	try {
        const data = await getData(`Course/participants/${course.Code}`);
        console.log(data);
        data.forEach(participants => {
        newcontent += `
                <div>
                    <p>${participants.Name}</p>
                </div>
            `;
        });
        newcontent += 
            `</div>`;
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }    
}*/
async function showCourses() {
    try {
        const data = await getData("Course");
        console.log(data);

        let newcontent = `
            <div>
                <h3>Kurzusok:</h3>
        `;
        data.forEach(course => {
            newcontent += `
                <div>
                    <h4>${course.Name}</h4>
                    <p><strong>Neptun kód:</strong> ${course.Code}</p>
                    <p><strong>Tanszék:</strong> ${course.Department}</p>
                    <p><strong>Kredit:</strong> ${course.Credit}</p>
                    <div>
                        <p><input type="button" value="Hallgatók listázása" onclick="showStudents('${course.Code}')"></p>
                        <p><input type="button" value="Kurzus jelentkezés" onclick="enrollStudents('${course.Code}')"></p>                        
                    </div>
                </div>
            `;
        });
        newcontent += `
            </div>`;
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

async function showStudents(courseCode) {
    let newcontent = `
        <h2>Hallgatók (${courseCode}):</h2>
        <div>
    `;
    try {
        const data = await getData(`Course/participants/${courseCode}`);
        console.log(data);
        data.forEach(participant => {
            newcontent += `
                <div>
                    <p>${participant.Name}</p>
                </div>
            `;
        });
        newcontent += `
            </div>`;
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

async function enrollStudents(courseCode) {
    console.log(courseCode)
    let newcontent = "";
    try {
        const data = putData(`Course/enroll/${courseCode}/${username}`);       
        console.log(data);

        newcontent = `
            <div>
                <h2>Sikeres jelentkezés.</h2>
            </div>
        `;
        document.getElementById("mainContent").innerHTML = newcontent
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }    
}

async function showEvents() {
	let newcontent = `
        <h2>Következő esemény</h2>
    `;
	try {
        const eventdata = await getData(`User/nextevent/${username}`);
        console.log(eventdata);
        eventdata.forEach(events => {
        newcontent += `
                <div>
                    <h4>${events.Name}</h4>
                    <p><strong>Kurzus(egyenlőre kód, nemtom hogy kéne nevet):</strong> ${events.Course_Id}</p>
                    <p><strong>Leírás:</strong> ${events.Description}</p>
                    <p><strong>Kredit:</strong> ${events.Date}</p>
                </div>
            `;
        });
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}



async function showCoursesByDept(){
    //var textArea = document.getElementById("mainContent");
    //var dept = document.getElementById("deptInput").value;
    try{
        const data = await getData(`Course/dept/${document.getElementById("deptInput").value}`);
        console.log(data);
        var newcontent = 
			`<div>
                    <h3>Kurzusok a(z) ${document.getElementById("deptInput").value} tankszéken:</h3>
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
        document.getElementById("mainContent").innerHTML = newcontent;
    } catch (error) {
        console.log("Adatbekérési hiba: " + error);
    }
}

function redirectLoginPage() {
	document.getElementById("logoutButton").addEventListener("click", function() {window.location.href = "index.html";});
		
}

async function showMyCourses() {
    var newcontent = `
        <h2>Saját Kurzusok</h2>
        <ul>
    `;
    const data = await getData(`Course/my/${username}`);
    console.log(data);

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
        `</ul>`;
    document.getElementById("mainContent").innerHTML = newcontent;
}

/*function redirectLoginPage() {
    document.getElementById("redirectButton").addEventListener("click", function() {window.location.href = "index.html";});

}*/
