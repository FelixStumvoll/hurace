import { useCallback, useContext } from 'react';
import { ThemeContext } from 'styled-components';
import { Theme } from 'react-select';

export const useSelectTheme = () => {
    const huraceTheme = useContext(ThemeContext);

    return useCallback(
        (theme: Theme) => ({
            ...theme,
            colors: { ...theme.colors, primary: huraceTheme.blue }
        }),
        // eslint-disable-next-line react-hooks/exhaustive-deps
        []
    );
};
