import React from 'react';
import styled from 'styled-components';
import { HeaderCard } from './HeaderCard';

const PageWrapper = styled.div`
    width: 100%;
    height: 100%;
    display: flex;
`;

const Fields = styled.div<{ rowCount: number }>`
    display: grid;
    grid-template-rows: repeat(auto ${props => props.rowCount});
    grid-template-columns: auto auto;
    gap: 10px;
`;

const HeaderCardWrapper = styled.div`
    margin: 50px auto auto auto;
`;

const CardContent = styled.div`
    display: flex;
    flex-direction: column;
`;

const CrudButton = styled.button`
    border-radius: 5px;
    border: none;
    color: white;
    height: 30px;
    cursor: pointer;
    flex-grow: 1;
`;

const CancelButton = styled(CrudButton)`
    margin-right: 5px;
    background-color: #e25959;
`;

const SaveButton = styled(CrudButton)`
    margin-left: 5px;
    background-color: #5f985f;
`;

const ButtonPanel = styled.div`
    display: flex;
    justify-content: center;
    margin-top: 15px;
`;

export const UpdateViewWrapper: React.FC<{
    headerText: string;
    rowCount: number;
    onSave: () => void;
    onCancel: () => void;
}> = ({ children, headerText, onSave, onCancel, rowCount }) => {
    return (
        <PageWrapper>
            <HeaderCardWrapper>
                <HeaderCard headerText={headerText}>
                    <CardContent>
                        <Fields rowCount={rowCount}>{children}</Fields>
                        <ButtonPanel>
                            <CancelButton onClick={onCancel}>
                                Abbrechen
                            </CancelButton>{' '}
                            <SaveButton onClick={onSave}>Speichern</SaveButton>
                        </ButtonPanel>
                    </CardContent>
                </HeaderCard>
            </HeaderCardWrapper>
        </PageWrapper>
    );
};
