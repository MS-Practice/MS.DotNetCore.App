function sayHello() {
    const compiler = (document.getElementById("compiler") as HTMLInputElement).value;
    const framwork = (document.getElementById("framework") as HTMLInputElement).value;
    return `Hello from ${compiler} and ${framwork}!`;
}