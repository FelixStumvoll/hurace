import React, { useContext } from 'react';
import { GridLoader } from 'react-spinners';
import styled, { ThemeContext } from 'styled-components';
import { FormErrorMessage } from '../../theme/CustomComponents';

const Host = styled.div`
    width: 100%;
    height: 100%;
    display: flex;
`;

const Center = styled.div`
    margin: auto;
`;

export const LoadingWrapper: React.FC<{
    loading: boolean;
    error: Error | undefined;
    errorMessage?: string;
}> = ({ errorMessage = 'Fehler beim Laden', loading, error, children }) => {
    const theme = useContext(ThemeContext);

    if (!error && !loading) return <> {children}</>;

    return (
        <Host>
            <Center>
                {loading && (
                    <GridLoader color={theme.blue} size={35}></GridLoader>
                )}
                {error && <FormErrorMessage>{errorMessage}</FormErrorMessage>}
            </Center>
        </Host>
    );
};
