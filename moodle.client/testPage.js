async function test(){
    var textArea = document.getElementById("kimenet");
    try{
        //console.log("1");
        const data = await getData("Test");
        //console.log("2");
        console.log(data);
        //console.log("3");
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}
async function allcourses(){
    var textArea = document.getElementById("kimenet");
    try{
        //console.log("1");
        const data = await getData("Course");
        //console.log("2");
        console.log(data);
        //console.log("3");
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}
async function dept(){
    var textArea = document.getElementById("kimenet");
    var dept = document.getElementById("dept").value;
    try{
        //console.log("1");
        const data = await getData("Course/dept/"+dept);
        //console.log("2");
        console.log(data);
        //console.log("3");
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}
async function mycourses(){
    var textArea = document.getElementById("kimenet");
    var neptun = document.getElementById("neptun").value;
    try{
        //console.log("1");
        const data = await getData("Course/mycourses/"+neptun);
        //console.log("2");
        console.log(data);
        //console.log("3");
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}
async function enroll(){
    var textArea = document.getElementById("kimenet");
    var targykod = document.getElementById("targykod").value;
    try{
        //console.log("1");
        var data = await fetch("http://localhost:5096/api/Course/enroll/"+targykod,
        {
            method: "POST", // *GET, POST, PUT, DELETE, etc.
            mode: "cors", // no-cors, *cors, same-origin
            cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
            credentials: "same-origin", // include, *same-origin, omit
            headers: {
                "Content-Type": "application/json"
            },
            redirect: "follow", // manual, *follow, error
            referrerPolicy: "no-referrer"
        }
        );
        //console.log("2");
        console.log(data);
        //console.log("3");
        //textArea.textContent = JSON.stringify(data, null, 2);
        textArea.textContent=data.value;
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}
async function nextevent(){
    var textArea = document.getElementById("kimenet");
    try{
        //console.log("1");
        const data = await getData("User");
        //console.log("2");
        console.log(data);
        //console.log("3");
        textArea.textContent = JSON.stringify(data, null, 2);
    }
    catch(error){
        console.log("Adatbekérési hiba: " + error);
    }
}