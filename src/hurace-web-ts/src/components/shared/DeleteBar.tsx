import React, { useState, useCallback, useContext } from 'react';
import styled, { ThemeContext } from 'styled-components';
import { DefaultButton } from '../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

const Bar = styled.div`
    margin-left: auto;
`;

const PositiveButton = styled(DefaultButton)<{ marginLeft: string }>`
    color: white;
    height: 100%;
    margin-left: ${props => props.marginLeft};
    background-color: ${props => props.theme.positive};
`;

const NegativeButton = styled(DefaultButton)<{ marginLeft: string }>`
    color: white;
    height: 100%;
    margin-left: ${props => props.marginLeft};
    background-color: ${props => props.theme.negative};
`;

const DeleteIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const DeleteQuestion = styled.span`
    margin-right: ${props => props.theme.gap};
    font-weight: bold;
`;

export const DeleteBar: React.FC<{
    deleteText: string;
    deleteFunc: () => void;
}> = ({ deleteText, deleteFunc }) => {
    const [deleteChallenge, setDeleteChallenge] = useState(false);

    const initDelete = useCallback(() => setDeleteChallenge(true), []);
    const cancelDelete = useCallback(() => setDeleteChallenge(false), []);

    const theme = useContext(ThemeContext);

    return (
        <Bar>
            {deleteChallenge ? (
                <>
                    <DeleteQuestion>Löschen ?</DeleteQuestion>
                    <PositiveButton marginLeft={theme.gap} onClick={deleteFunc}>
                        Ja
                    </PositiveButton>
                    <NegativeButton
                        marginLeft={theme.gap}
                        onClick={cancelDelete}
                    >
                        Nein
                    </NegativeButton>
                </>
            ) : (
                <NegativeButton marginLeft="0px" onClick={initDelete}>
                    <DeleteIcon icon={faTrash} />
                    {deleteText}
                </NegativeButton>
            )}
        </Bar>
    );
};
