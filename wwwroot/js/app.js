const parkingApi = "/api/parking";
const sessionsApi = "/api/sessions";
const authApi = "/api/auth";

const parkingForm = document.getElementById("parkingForm");
const sessionForm = document.getElementById("sessionForm");
const loginForm = document.getElementById("loginForm");
const registerForm = document.getElementById("registerForm");
const logoutButton = document.getElementById("logoutButton");
const loginNav = document.getElementById("loginNav");
const registerNav = document.getElementById("registerNav");

const spotIdInput = document.getElementById("spotId");
const spotNameInput = document.getElementById("spotName");
const priceInput = document.getElementById("pricePerHour");
const availableInput = document.getElementById("isAvailable");
const submitButton = document.getElementById("submitButton");
const cancelEditButton = document.getElementById("cancelEditButton");
const parkingFormTitle = document.getElementById("parkingFormTitle");
const searchInput = document.getElementById("searchInput");
const statusFilter = document.getElementById("statusFilter");
const refreshButton = document.getElementById("refreshButton");
const sessionSpotSelect = document.getElementById("sessionSpotId");
const tableBody = document.getElementById("parkingTableBody");
const sessionsTableBody = document.getElementById("sessionsTableBody");
const message = document.getElementById("message");
const currentUser = document.getElementById("currentUser");

const totalCount = document.getElementById("totalCount");
const availableCount = document.getElementById("availableCount");
const occupiedCount = document.getElementById("occupiedCount");
const activeSessionCount = document.getElementById("activeSessionCount");
const availableList = document.getElementById("availableList");
const occupiedList = document.getElementById("occupiedList");

let parkingSpots = [];
let sessions = [];

function isAuthPage() {
    const page = window.location.pathname.split("/").pop().toLowerCase();
    return page === "login.html" || page === "register.html";
}

function isHomePage() {
    const page = window.location.pathname.split("/").pop().toLowerCase();
    return page === "" || page === "index.html";
}

function requireAuthenticatedAccess() {
    const user = getUser();

    if (!user && isHomePage()) {
        window.location.replace("login.html");
        return false;
    }

    if (user && isAuthPage()) {
        window.location.replace("index.html");
        return false;
    }

    return true;
}

function showMessage(text, type = "") {
    if (!message)
        return;

    message.textContent = text;
    message.className = `message ${type}`.trim();
}

function getUser() {
    const user = localStorage.getItem("smartParkingUser");
    try {
        return user ? JSON.parse(user) : null;
    } catch {
        localStorage.removeItem("smartParkingUser");
        return null;
    }
}

function setUser(user) {
    localStorage.setItem("smartParkingUser", JSON.stringify(user));
    renderUser();
}

function isAdmin() {
    return getUser()?.role?.toLowerCase() === "admin";
}

function toggleAdminUi() {
    document.querySelectorAll("[data-admin-only]").forEach((element) => {
        element.hidden = !isAdmin();
    });
}

function renderUser() {
    const user = getUser();

    if (currentUser)
        currentUser.textContent = user ? `${user.fullName} (${user.role})` : "Nuk je kycur";

    if (logoutButton)
        logoutButton.hidden = !user;

    if (loginNav)
        loginNav.hidden = !!user;

    if (registerNav)
        registerNav.hidden = !!user;

    toggleAdminUi();
}

function clearAuthForms() {
    if (!isAuthPage())
        return;

    loginForm?.reset();
    registerForm?.reset();
    document.getElementById("loginEmail")?.setAttribute("value", "");
    document.getElementById("loginPassword")?.setAttribute("value", "");
}

function authHeaders(extraHeaders = {}) {
    const user = getUser();

    return {
        ...extraHeaders,
        ...(user?.token ? { Authorization: `Bearer ${user.token}` } : {})
    };
}

async function apiFetch(url, options = {}) {
    const response = await fetch(url, {
        ...options,
        headers: authHeaders(options.headers || {})
    });

    if (response.status === 401) {
        localStorage.removeItem("smartParkingUser");
        renderUser();
        window.location.replace("login.html");
        throw new Error("Sesioni skadoi. Kycu perseri.");
    }

    return response;
}

function parkingPayload() {
    return {
        id: Number(spotIdInput.value) || 0,
        name: spotNameInput.value.trim(),
        pricePerHour: Number(priceInput.value),
        isAvailable: availableInput.checked
    };
}

function resetParkingForm() {
    if (!parkingForm)
        return;

    parkingForm.reset();
    spotIdInput.value = "";
    availableInput.checked = true;
    submitButton.textContent = "Shto";
    parkingFormTitle.textContent = "Shto parking";
    cancelEditButton.hidden = true;
}

function renderStats(spots) {
    if (!totalCount || !availableCount || !occupiedCount || !activeSessionCount)
        return;

    const available = spots.filter((spot) => spot.isAvailable).length;
    const activeSessions = sessions.filter((session) => session.isActive).length;

    totalCount.textContent = spots.length;
    availableCount.textContent = available;
    occupiedCount.textContent = spots.length - available;
    activeSessionCount.textContent = activeSessions;
    renderAvailabilityLists(spots);
}

function renderAvailabilityLists(spots) {
    renderSpotList(availableList, spots.filter((spot) => spot.isAvailable), "Nuk ka parkingje te lira.");
    renderSpotList(occupiedList, spots.filter((spot) => !spot.isAvailable), "Nuk ka parkingje te zena.");
}

function renderSpotList(container, spots, emptyText) {
    if (!container)
        return;

    if (spots.length === 0) {
        container.innerHTML = `<p class="empty-note">${emptyText}</p>`;
        return;
    }

    container.innerHTML = spots.map((spot) => `
        <div class="spot-list-item">
            <strong>${spot.name}</strong>
            <span>${Number(spot.pricePerHour).toFixed(2)} EUR/ore</span>
        </div>
    `).join("");
}

function renderAvailableSpotOptions() {
    if (!sessionSpotSelect)
        return;

    const selectedValue = sessionSpotSelect.value;
    const availableSpots = parkingSpots.filter((spot) => spot.isAvailable);

    sessionSpotSelect.innerHTML = [
        `<option value="">Zgjidh parkingun</option>`,
        ...availableSpots.map((spot) =>
            `<option value="${spot.id}">${spot.name} - ${Number(spot.pricePerHour).toFixed(2)} EUR/ore</option>`)
    ].join("");

    if (availableSpots.some((spot) => String(spot.id) === selectedValue))
        sessionSpotSelect.value = selectedValue;
}

function filteredSpots() {
    if (!searchInput || !statusFilter)
        return parkingSpots;

    const term = searchInput.value.trim().toLowerCase();
    const status = statusFilter.value;

    return parkingSpots.filter((spot) => {
        const matchesSearch = spot.name.toLowerCase().includes(term);
        const matchesStatus =
            status === "all" ||
            (status === "available" && spot.isAvailable) ||
            (status === "occupied" && !spot.isAvailable);

        return matchesSearch && matchesStatus;
    });
}

function renderParkingTable() {
    if (!tableBody)
        return;

    const spots = filteredSpots();
    const showAdminActions = isAdmin();
    renderStats(parkingSpots);

    if (spots.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="${showAdminActions ? 5 : 4}">Nuk u gjet asnje parking.</td></tr>`;
        return;
    }

    tableBody.innerHTML = spots.map((spot) => `
        <tr>
            <td>${spot.id}</td>
            <td>${spot.name}</td>
            <td>${Number(spot.pricePerHour).toFixed(2)} EUR</td>
            <td>
                <span class="badge ${spot.isAvailable ? "available" : "occupied"}">
                    ${spot.isAvailable ? "I lire" : "I zene"}
                </span>
            </td>
            ${showAdminActions ? `<td>
                <div class="row-actions">
                    <button class="edit-button" data-action="edit" data-id="${spot.id}">Edit</button>
                    <button class="delete-button" data-action="delete" data-id="${spot.id}">Delete</button>
                </div>
            </td>` : ""}
        </tr>
    `).join("");
}

function renderSessionsTable() {
    if (!sessionsTableBody)
        return;

    renderStats(parkingSpots);

    if (sessions.length === 0) {
        sessionsTableBody.innerHTML = `<tr><td colspan="7">Nuk ka sesione parkimi.</td></tr>`;
        return;
    }

    sessionsTableBody.innerHTML = sessions.map((session) => `
        <tr>
            <td>${session.id}</td>
            <td>${session.parkingSpotId}</td>
            <td>${session.vehiclePlate}</td>
            <td>${formatDate(session.entryTime)}</td>
            <td>${session.exitTime ? formatDate(session.exitTime) : "Aktiv"}</td>
            <td>${Number(session.totalPrice).toFixed(2)} EUR</td>
            <td>
                ${session.isActive
                    ? `<button class="end-button" data-action="end-session" data-id="${session.id}">Mbyll</button>`
                    : `<span class="badge available">Mbyllur</span>`}
            </td>
        </tr>
    `).join("");
}

function formatDate(value) {
    return new Date(value).toLocaleString("sq-AL", {
        dateStyle: "short",
        timeStyle: "short"
    });
}

async function loadParkingSpots() {
    const response = await apiFetch(parkingApi);

    if (!response.ok)
        throw new Error("Parkingjet nuk u lexuan");

    parkingSpots = await response.json();
    renderParkingTable();
    renderAvailableSpotOptions();
}

async function loadSessions() {
    const response = await apiFetch(sessionsApi);

    if (!response.ok)
        throw new Error("Sesionet nuk u lexuan");

    sessions = await response.json();
    renderSessionsTable();
}

async function refreshData() {
    if (!getUser())
        return;

    if (!tableBody && !sessionsTableBody)
        return;

    try {
        await loadParkingSpots();
        await loadSessions();
    } catch (error) {
        showMessage(error.message, "error");
    }
}

async function saveParkingSpot(event) {
    event.preventDefault();

    const spot = parkingPayload();

    if (!spot.name || spot.pricePerHour <= 0 || spot.pricePerHour > 1000) {
        showMessage("Ploteso emrin dhe cmimin valid nga 0.01 deri 1000 EUR.", "error");
        return;
    }

    const isEditing = spot.id > 0;
    const existingSpot = parkingSpots.find((item) =>
        item.id !== spot.id &&
        item.name.toLowerCase() === spot.name.toLowerCase());

    if (existingSpot) {
        showMessage(
            `${existingSpot.name} ekziston dhe eshte ${existingSpot.isAvailable ? "i lire" : "i zene"}. Nuk mund te shtohet perseri.`,
            "error");
        return;
    }

    const response = await apiFetch(isEditing ? `${parkingApi}/${spot.id}` : parkingApi, {
        method: isEditing ? "PUT" : "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(spot)
    });

    if (!response.ok) {
        const error = await response.json().catch(() => null);
        showMessage(error?.message || "Ruajtja deshtoi.", "error");
        return;
    }

    resetParkingForm();
    await refreshData();
    showMessage(isEditing ? "Parkingu u perditesua." : "Parkingu u shtua.", "success");
}

function startEdit(id) {
    const spot = parkingSpots.find((item) => item.id === id);

    if (!spot)
        return;

    spotIdInput.value = spot.id;
    spotNameInput.value = spot.name;
    priceInput.value = spot.pricePerHour;
    availableInput.checked = spot.isAvailable;
    submitButton.textContent = "Ruaj ndryshimet";
    parkingFormTitle.textContent = "Perditeso parking";
    cancelEditButton.hidden = false;
    spotNameInput.focus();
}

async function deleteParkingSpot(id) {
    const spot = parkingSpots.find((item) => item.id === id);

    if (!confirm(`A deshiron ta fshish ${spot?.name || "parkingun"}?`))
        return;

    const response = await apiFetch(`${parkingApi}/${id}`, { method: "DELETE" });

    if (!response.ok) {
        showMessage("Fshirja deshtoi.", "error");
        return;
    }

    await refreshData();
    showMessage("Parkingu u fshi.", "success");
}

async function startSession(event) {
    event.preventDefault();

    const parkingSpotId = Number(sessionSpotSelect?.value);
    const vehiclePlate = document.getElementById("vehiclePlate").value.trim().replace(/\s+/g, "").toUpperCase();

    if (!parkingSpotId || !vehiclePlate) {
        showMessage("Zgjidh parkingun dhe ploteso targen.", "error");
        return;
    }

    const response = await apiFetch(`${sessionsApi}/start`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ parkingSpotId, vehiclePlate })
    });

    if (!response.ok) {
        const error = await response.json().catch(() => null);
        showMessage(error?.message || "Hyrja nuk u regjistrua.", "error");
        return;
    }

    sessionForm.reset();
    await refreshData();
    showMessage("Hyrja e automjetit u regjistrua.", "success");
}

async function endSession(id) {
    const response = await apiFetch(`${sessionsApi}/${id}/end`, { method: "PUT" });

    if (!response.ok) {
        const error = await response.json().catch(() => null);
        showMessage(error?.message || "Dalja nuk u regjistrua.", "error");
        return;
    }

    await refreshData();
    showMessage("Dalja u regjistrua dhe pagesa u kalkulua.", "success");
}

async function login(event) {
    event.preventDefault();

    const response = await fetch(`${authApi}/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            email: document.getElementById("loginEmail").value.trim(),
            password: document.getElementById("loginPassword").value
        })
    });

    if (!response.ok) {
        const error = await response.json().catch(() => null);
        showMessage(error?.message || "Login deshtoi.", "error");
        return;
    }

    setUser(await response.json());
    loginForm.reset();
    showMessage("U kyce me sukses.", "success");
    window.location.href = "index.html";
}

async function register(event) {
    event.preventDefault();

    const response = await fetch(`${authApi}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            fullName: document.getElementById("registerName").value.trim(),
            email: document.getElementById("registerEmail").value.trim(),
            password: document.getElementById("registerPassword").value
        })
    });

    if (!response.ok) {
        const error = await response.json().catch(() => null);
        showMessage(error?.message || "Register deshtoi.", "error");
        return;
    }

    setUser(await response.json());
    registerForm.reset();
    showMessage("Llogaria u krijua.", "success");
    window.location.href = "index.html";
}

parkingForm?.addEventListener("submit", saveParkingSpot);
sessionForm?.addEventListener("submit", startSession);
loginForm?.addEventListener("submit", login);
registerForm?.addEventListener("submit", register);
cancelEditButton?.addEventListener("click", resetParkingForm);
searchInput?.addEventListener("input", renderParkingTable);
statusFilter?.addEventListener("change", renderParkingTable);
refreshButton?.addEventListener("click", async () => {
    await refreshData();
    showMessage("Te dhenat u rifreskuan.", "success");
});
logoutButton?.addEventListener("click", async () => {
    await apiFetch(`${authApi}/logout`, { method: "POST" }).catch(() => null);
    localStorage.removeItem("smartParkingUser");
    renderUser();
    showMessage("Dole nga llogaria.", "success");
    window.location.href = "login.html";
});

tableBody?.addEventListener("click", (event) => {
    const button = event.target.closest("button[data-action]");

    if (!button)
        return;

    const id = Number(button.dataset.id);

    if (button.dataset.action === "edit")
        startEdit(id);

    if (button.dataset.action === "delete")
        deleteParkingSpot(id);
});

sessionsTableBody?.addEventListener("click", (event) => {
    const button = event.target.closest("button[data-action='end-session']");

    if (button)
        endSession(Number(button.dataset.id));
});

if (requireAuthenticatedAccess()) {
    clearAuthForms();
    renderUser();
    refreshData();
}
