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