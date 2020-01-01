export const setStateAsync = <T>(
    setFunc: (val: T) => void,
    valuePromise: Promise<T>
) => {
    const asyncWrapper = async () => setFunc(await valuePromise);
    asyncWrapper();
};
