import { useState, useEffect, useCallback } from 'react';

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

    const asyncWrapper = useCallback(async () => {
        setIsLoading(true);
        setIsError(false);
        try {
            setTVal(await promiseFunc(...args));
        } catch (error) {
            setIsError(true);
        }

        setIsLoading(false);
    }, [args, promiseFunc]);

    useEffect(() => {
        if (isError || tVal || isLoading) return;

        asyncWrapper();
    }, [asyncWrapper, isError, tVal, isLoading]);

    return [tVal, setTVal, isLoading, isError];
};
