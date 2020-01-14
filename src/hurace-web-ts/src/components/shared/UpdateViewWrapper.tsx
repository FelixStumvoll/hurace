import React from 'react';
import styled from 'styled-components';
import { HeaderCard } from './HeaderCard';

const PageWrapper = styled.div`
    width: 100%;
    height: 100%;
    display: flex;
`;

const HeaderCardWrapper = styled.div`
    margin: 50px auto auto auto;
`;

export const UpdateViewWrapper: React.FC<{
    headerText: string;
    errorText?: string;
}> = ({ children, headerText }) => {
    return (
        <PageWrapper>
            <HeaderCardWrapper>
                <HeaderCard headerText={headerText}>
                    {children}
                </HeaderCard>
            </HeaderCardWrapper>
        </PageWrapper>
    );
};
