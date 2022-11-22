export const awaitTask = new Promise((r) => {
    console.log(new Date(), '555');
    setTimeout(() => {
        console.log(new Date(), '444');
        r(new Date());
    }, 10000);
});