import { useState, useEffect } from 'react';

export const useStateAsync = <T>(
    promiseFunc: (...args: any[]) => Promise<T>,
    ...args: any[]
): [
    T | undefined,
    React.Dispatch<React.SetStateAction<T | undefined>>,
    boolean,
    boolean
] => {
    const [tVal, setTVal] = useState<T>();
    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);

    useEffect(() => {
        if (tVal || isLoading) return;
        const asyncWrapper = async () => {
            setIsLoading(true);
            setIsError(false);
            try {
                setTVal(await promiseFunc(...args));
            } catch (error) {
                setIsError(true);
            }

            setIsLoading(false);
        };
        asyncWrapper();
    }, [tVal, promiseFunc, args, isLoading]);

    return [tVal, setTVal, isLoading, isError];
};
