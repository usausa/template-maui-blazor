export function ping() {
}

export function getInformation() {
    let size = 0;
    for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        size += key.length + (localStorage.getItem(key) ?? '').length;
    }

    return {
        userAgent: navigator.userAgent,
        online: navigator.onLine,
        storageCount: localStorage.length,
        storageSize: size * 2
    };
}
