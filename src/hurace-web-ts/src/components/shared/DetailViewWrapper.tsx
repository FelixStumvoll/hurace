import React from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleLeft, faPen } from '@fortawesome/free-solid-svg-icons';
import { DefaultLink, RowFlex, AlignRight } from '../../theme/CustomComponents';
import { AuthContext } from '../Auth0Provider';
import { DeleteModalHost } from './DeleteModalHost';

const BackLinkPanel = styled.div`
    display: grid;
    grid-template-rows: auto 1fr;
    row-gap: ${props => props.theme.gap};
    height: 100%;
`;

const EditText = styled.div`
    margin: auto;
    padding: 0 10px 0 10px;
`;

const EditLink = styled(DefaultLink)<{ deletepresent: number }>`
    margin-left: ${props =>
        props.deletepresent === 0 ? props.theme.gap : 'auto'};
    border-radius: 5px;
    background-color: #f0db4f;
    color: black;
    display: flex;
`;

const EditIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const BackLink = styled(DefaultLink)`
    font-size: 20px;
    display: flex;
`;

const BackIcon = styled(FontAwesomeIcon)`
    margin: auto 5px auto 0;
`;

const BackText = styled.div`
    margin: auto;
    height: fit-content;
`;

const ActionButtons = styled(AlignRight)`
    height: 35px;
    display: flex;
`;

export const DetailViewWrapper: React.FC<{
    backText: string;
    url: string;
    editConfig?: {
        editText: string;
        editUrl: string;
    };

    deleteConfig?: {
        deleteText: string;
        deleteFunc: () => Promise<void>;
    };
}> = ({ backText, url, children, editConfig, deleteConfig }) => {
    return (
        <BackLinkPanel>
            <RowFlex>
                <BackLink to={url}>
                    <BackIcon icon={faAngleLeft}></BackIcon>
                    <BackText>{backText}</BackText>
                </BackLink>

                <AuthContext.Consumer>
                    {({ isAuthenticated }) =>
                        isAuthenticated && (
                            <ActionButtons>
                                {deleteConfig && (
                                    <DeleteModalHost
                                        deleteText={deleteConfig.deleteText}
                                        deleteFunc={deleteConfig.deleteFunc}
                                    />
                                )}
                                {editConfig && (
                                    <EditLink
                                        deletepresent={!!deleteConfig ? 0 : 1}
                                        to={editConfig.editUrl}
                                    >
                                        <EditText>
                                            <EditIcon icon={faPen} />
                                            {editConfig.editText}
                                        </EditText>
                                    </EditLink>
                                )}
                            </ActionButtons>
                        )
                    }
                </AuthContext.Consumer>
            </RowFlex>

            {children}
        </BackLinkPanel>
    );
};
