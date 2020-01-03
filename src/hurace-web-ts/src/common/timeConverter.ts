const zeroPrepender = (val: number, zeros: number = 1): string => {
    let retString = val.toString();
    for (
        let prepended = 0;
        prepended < zeros && retString.length !== zeros + 1;
        prepended++
    )
        retString = `0${retString}`;
    return retString;
};

export const getSkierTimeString = (date: Date): string =>
    `${zeroPrepender(date.getMinutes())}:${zeroPrepender(
        date.getSeconds()
    )}:${zeroPrepender(date.getMilliseconds(), 2)}`;

export const getRaceTimeString = (date: Date): string =>
    `${zeroPrepender(date.getHours())}:${zeroPrepender(date.getMinutes())}`;
