import React from 'react';
import { Skier } from '../../interfaces/Skier';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

const TrLink = styled.tr`
    cursor: pointer;
`;

const TdBorder = styled.td`
    border-bottom: 1px solid ${props => props.theme.gray};
`;

export const SkierListItem: React.FC<{ skier: Skier }> = ({ skier }) => {
    let history = useHistory();
    return (
        <TrLink onClick={() => history.push(`/skier/${skier.id}`)}>
            <TdBorder> {skier.country.countryCode}</TdBorder>
            <TdBorder>
                {skier.firstName} {skier.lastName}
            </TdBorder>
            <TdBorder>{skier.gender.genderDescription}</TdBorder>
        </TrLink>
    );
};
