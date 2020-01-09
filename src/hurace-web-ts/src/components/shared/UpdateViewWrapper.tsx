import React from 'react';
import styled from 'styled-components';
import { HeaderCard } from './HeaderCard';
import { DefaultButton, ColumnFlex } from '../../theme/CustomComponents';

const PageWrapper = styled.div`
    width: 100%;
    height: 100%;
    display: flex;
`;

const HeaderCardWrapper = styled.div`
    margin: 50px auto auto auto;
`;

const CrudButton = styled(DefaultButton)`
    flex-grow: 1;
    color: white;
`;

const CancelButton = styled(CrudButton)`
    margin-right: 5px;
    background-color: ${props => props.theme.negative};
`;

const SaveButton = styled(CrudButton)`
    margin-left: 5px;
    background-color: ${props => props.theme.positive};

    :disabled {
        background-color: ${props => props.theme.gray};
        cursor: not-allowed;
    }
`;

const ErrorMessage = styled.div`
    border: 1px solid ${props => props.theme.negative};
    background-color: transparent;
    color: ${props => props.theme.negative};
`;

const ButtonPanel = styled.div`
    display: flex;
    justify-content: center;
    margin-top: 15px;
`;

export const UpdateViewWrapper: React.FC<{
    headerText: string;
    errorText?: string;
    onSave: () => void;
    onCancel: () => void;
    canSave?: () => boolean;
}> = ({ children, headerText, onSave, onCancel, canSave, errorText }) => {
    const saveDisabled = canSave ? !canSave() : false;

    return (
        <PageWrapper>
            <HeaderCardWrapper>
                <HeaderCard headerText={headerText}>
                    <ColumnFlex>
                        {children}
                        {errorText && <ErrorMessage>{errorText}</ErrorMessage>}
                        <ButtonPanel>
                            <CancelButton onClick={onCancel}>
                                Abbrechen
                            </CancelButton>
                            <SaveButton
                                disabled={saveDisabled}
                                onClick={onSave}
                            >
                                Speichern
                            </SaveButton>
                        </ButtonPanel>
                    </ColumnFlex>
                </HeaderCard>
            </HeaderCardWrapper>
        </PageWrapper>
    );
};
