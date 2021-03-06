import 'styled-components';

declare module 'styled-components' {
    export interface DefaultTheme {
        navHeight: string;
        gray: string;
        black: string;
        blue: string;
        negative: string;
        positive: string;
        fontFamily: string;
        gap: string;
        mobileSize: string;
        tabletSize: string;
    }
}
