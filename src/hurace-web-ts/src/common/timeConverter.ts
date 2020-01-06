const padMinutes = (date: Date): string =>
    date
        .getMinutes()
        .toString()
        .padStart(2, '0');

export const getTimeWithMS = (date: Date): string =>
    `${padMinutes(date)}:${date
        .getSeconds()
        .toString()
        .padStart(2, '0')}:${(date
        .getMilliseconds()
        .toString()
        .padStart(3, '0'),
    2)}`;

export const getTime = (date: Date): string =>
    `${date
        .getHours()
        .toString()
        .padStart(2, '0')}:${padMinutes(date)}`;

export const getDate = (date: Date) =>
    `${date
        .getDate()
        .toString()
        .padStart(2, '0')}.${(date.getMonth() + 1)
        .toString()
        .padStart(2, '0')}.${date.getFullYear()}`;
