import { useState } from 'react';

export const useCallState = (): {
    loading: boolean;
    error: Error | undefined;
    setLoading: React.Dispatch<React.SetStateAction<boolean>>;
    setError: React.Dispatch<React.SetStateAction<Error | undefined>>;
} => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error>();

    return { loading, error, setLoading, setError };
};
