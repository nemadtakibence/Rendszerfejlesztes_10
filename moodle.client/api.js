var defaultUrl = "http://localhost:5096/api/";

async function postData(url = "", data = {}, needAuth = true) {
    const headers = {
        "Content-Type": "application/json"
    };
    if (needAuth) {
        const token = localStorage.getItem('token');
        if (token) {
			headers['Authorization'] = `Bearer ${token}`;
        }
    }
    const response = await fetch(defaultUrl + url, {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: headers,
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify(data)
    });
    if (response.status === 401 || response.status === 403) {
        logout();
    }
    try {
        return await response.json();
    } catch (error) {
        console.error("Error parsing JSON:", error);
        return {};
    }
}

async function getData(url = "", needAuth = true) {
    const headers = {
        "Content-Type": "application/json"
    };
    if (needAuth) {
        const token = localStorage.getItem('token');
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
    }
    const response = await fetch(defaultUrl + url, {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: headers,
        redirect: "follow",
        referrerPolicy: "no-referrer"
    });
    if (response.status === 401 || response.status === 403) {
        logout();
    }
    console.log(response);
    return response.json();
}

async function putData(url = "", data = {}, needAuth = true) {
    const headers = {
        "Content-Type": "application/json"
    };
    if (needAuth) {
        const token = localStorage.getItem('token');
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
    }
    const response = await fetch(defaultUrl + url, {
        method: "PUT",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: headers,
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify(data)
    });
    if (response.status === 401 || response.status === 403) {
        logout();
    }
    try {
        return await response.json();
    } catch (error) {
        console.error("Error parsing JSON:", error);
        return {};
    }
}

function logout() {
    localStorage.clear();
    //window.location.href = "index.html";
}