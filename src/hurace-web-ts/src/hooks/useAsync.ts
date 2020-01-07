import { useState, useEffect, useCallback } from 'react';

export const useAsync = (
    promiseFunc: (...args: any[]) => Promise<void>,
    ...args: any[]
) => {
    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);

    const asyncWrapper = useCallback(async () => {
        setIsLoading(true);
        setIsError(false);
        try {
            await promiseFunc(...args);
        } catch (error) {
            setIsError(true);
        }

        setIsLoading(false);
    }, [args, promiseFunc]);

    useEffect(() => {
        if (isError || isLoading) return;

        asyncWrapper();
    }, [asyncWrapper, isError, isLoading]);
};
