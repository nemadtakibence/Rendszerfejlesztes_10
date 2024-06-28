/*async function login(username, password) {
    const loginData = { username: username, password: password };
    try {
        const response = await postData("Auth/login", loginData, false);
        if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('username', response.userName);
            window.location.href = "mainPage.html"; // Redirect to the home page
        } else {
			console.log("Full server response:", response);
            document.getElementById('sikertelenLogin').innerText = "Login failed: " + response.message;
        }
    } catch (error) {
        console.error("Error during login:", error);
        document.getElementById('sikertelenLogin').innerText = "Error during login: " + error;
    }
}

function loginAction() {
    var username = document.getElementById('usernameInput').value;
    var password = document.getElementById('passwordInput').value;
	console.log("Username:", username, "Password:", password); // Add this line
    login(username, password);
}
*/

async function hashPassword(username, password) {
    const encoder = new TextEncoder();
    const data = encoder.encode(username + password); // Combine username and password
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
    return hashBase64;
}

async function login(username, password) {
    const hashedPassword = await hashPassword(username, password); // Combine username before hashing
    const loginData = { username: username, password: hashedPassword };

    try {
        const response = await postData("Auth/login", loginData, false);
        if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('username', response.userName);
            window.location.href = "mainPage.html"; // Redirect to the home page
        } else {
            console.log("Full server response:", response);
            document.getElementById('sikertelenLogin').innerText = "Login failed: " + response.message;
        }
    } catch (error) {
        console.error("Error during login:", error);
        document.getElementById('sikertelenLogin').innerText = "Error during login: " + error;
    }
}

async function loginAction() {
    var username = document.getElementById('usernameInput').value;
    var password = document.getElementById('passwordInput').value;
    console.log("Username:", username, "Password:", password); // Add this line
    await login(username, password);
}
