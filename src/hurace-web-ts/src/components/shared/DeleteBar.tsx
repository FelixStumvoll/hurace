import React, { useState, useCallback } from 'react';
import styled from 'styled-components';
import { DefaultButton } from '../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

const Bar = styled.div`
    margin-left: auto;
`;

const PositiveButton = styled(DefaultButton)<{ marginLeft: number }>`
    color: white;
    height: 100%;
    margin-left: ${props => props.marginLeft}px;
    background-color: ${props => props.theme.positive};
`;

const NegativeButton = styled(DefaultButton)<{ marginLeft: number }>`
    color: white;
    height: 100%;
    margin-left: ${props => props.marginLeft}px;
    background-color: ${props => props.theme.negative};
`;

const DeleteIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const DeleteQuestion = styled.span`
    margin-right: 10px;
    font-weight: bold;
`;

export const DeleteBar: React.FC<{
    deleteText: string;
    deleteFunc: () => void;
}> = ({ deleteText, deleteFunc }) => {
    const [deleteChallenge, setDeleteChallenge] = useState(false);

    const initDelete = useCallback(() => setDeleteChallenge(true), []);
    const cancelDelete = useCallback(() => setDeleteChallenge(false), []);

    return (
        <Bar>
            {deleteChallenge ? (
                <>
                    <DeleteQuestion>Löschen ?</DeleteQuestion>
                    <PositiveButton marginLeft={10} onClick={deleteFunc}>
                        Ja
                    </PositiveButton>
                    <NegativeButton marginLeft={10} onClick={cancelDelete}>
                        Nein
                    </NegativeButton>
                </>
            ) : (
                <NegativeButton marginLeft={0} onClick={initDelete}>
                    <DeleteIcon icon={faTrash} />
                    {deleteText}
                </NegativeButton>
            )}
        </Bar>
    );
};
